use std::{os::raw::c_char, collections::{HashMap, HashSet}};

use ast::Expr;
use diagnostic::Diagnostic;
use std::f64;

#[repr(C)]
pub struct VariablePair {
    pub name: *const c_char,
    pub value: f64,
}

#[repr(C)]
pub struct VariablePairArray {
    pub count: i32,
    pub pairs: *const VariablePair,
}

#[repr(C)]
pub struct IdItem {
    pub name: *const c_char,
}

#[repr(C)]
pub struct ParseResult {
    pub ast_id: i32,
    pub diagnostics: *const LowDiagnostic,
    pub diagnostics_count: i32,
}

#[repr(C)]
pub struct LowDiagnostic {
    level: diagnostic::Level,
    message: *const c_char,
    message_length: i32,
}

static mut AST_MAP: Option<HashMap<i32, Box<Expr>>> = None;

fn ast_map() -> &'static mut HashMap<i32, Box<Expr>> {
    if let Some(map) = unsafe { AST_MAP.as_mut() } {
        map
    } else {
        unsafe {
            AST_MAP = Some(HashMap::new());
            AST_MAP.as_mut().unwrap()
        }
    }
}

static mut NEXT_ID: i32 = 1;

static mut LOW_DIAGNOSTICS: Option<Vec<LowDiagnostic>> = None;

fn low_diagnostics() -> &'static mut Vec<LowDiagnostic> {
    if let Some(diagnostics) = unsafe { LOW_DIAGNOSTICS.as_mut() } {
        diagnostics
    } else {
        unsafe {
            LOW_DIAGNOSTICS = Some(Vec::new());
            LOW_DIAGNOSTICS.as_mut().unwrap()
        }
    }
}

fn register_ast(ast: Box<Expr>) -> i32 {
    let id = unsafe { NEXT_ID };
    ast_map().insert(id, ast);
    unsafe {
        NEXT_ID += 1;
    }
    id
}

#[no_mangle]
pub fn dispose_ast(id: i32) {
    ast_map().remove(&id);
}

fn get_ast(id: i32) -> &'static Box<Expr> {
    ast_map().get(&id).unwrap()
}

lazy_static! {
    pub static ref CONSTANTS: HashMap<String, f64> = HashMap::from([
        ("e".to_string(), f64::consts::E),
        ("pi".to_string(), f64::consts::PI),
        ("ln2".to_string(), f64::consts::LN_2),
        ("ln10".to_string(), f64::consts::LN_10),
        ("sqrt2".to_string(), f64::consts::SQRT_2),
    ]);
}

#[no_mangle]
pub extern fn create_ast(
    input: *const c_char,
    id_item_count: i32,
    id_items: *const IdItem,
) -> ParseResult {
    Diagnostic::clear();

    let input = unsafe {
        std::ffi::CStr::from_ptr(input).to_str().unwrap()
    };

    let id_items = unsafe {
        std::slice::from_raw_parts(id_items, id_item_count as usize)
    }.iter().map(|item| {
        unsafe {
            std::ffi::CStr::from_ptr(item.name).to_str().unwrap().to_string()
        }
    }).collect::<HashSet<_>>();

    let token_iter = lexer::token_iter(&input);

    let ast = parser::parse_top_level_expression(
        parser::ParserContext::new(
            Box::new(token_iter),
            parser::create_binary_op_precedence()
        )
    );

    let mut result = -1;

    if let Ok(ast) = ast {
        if validator::validate_number_equation(
            &ast, 
            &CONSTANTS,
            &HashMap::new(),
            &id_items
        ) {
            let id = register_ast(ast);
            result = id;
        }
    }

    let low_diagnostics = low_diagnostics();
    low_diagnostics.clear();

    for diagnostic in Diagnostic::diagnostics().to_vec().iter() {
        low_diagnostics.push(LowDiagnostic {
            level: diagnostic.level(),
            message: diagnostic.message().as_ptr() as *const c_char,
            message_length: diagnostic.message().len() as i32
        });
    }

    let low_diagnostics_ptr = low_diagnostics.as_ptr();
    let diagnostics_len = low_diagnostics.len();

    ParseResult {
        ast_id: result,
        diagnostics: low_diagnostics_ptr,
        diagnostics_count: diagnostics_len as i32,
    }
}

#[no_mangle]
pub extern fn eval_ast(
    ast_id: i32,
    variables: VariablePairArray
) -> f64 {
    let ast = get_ast(ast_id);
    let variables = unsafe {
        std::slice::from_raw_parts(variables.pairs, variables.count as usize)
    };
    let variables = variables.iter().map(|pair| {
        (
            unsafe {std::ffi::CStr::from_ptr(pair.name).to_str().unwrap().to_string() },
            pair.value
        )
    }).collect::<HashMap<String, f64>>();

    let eval_result = evaluator::fold_const_expr(
        &ast,
        &CONSTANTS,
        &variables
    );
    
    eval_result
}

/*
create_ast (*char8 input, int32 id_item_count, *IdItem id_items, *char8 output)
dispose_ast
eval_ast
 */
