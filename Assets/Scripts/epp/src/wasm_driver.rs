use std::collections::{HashMap, HashSet};

use lexer;
use parser;
use diagnostic;
use transpiler;
use wasm_bindgen::prelude::*;
use std::f64;

use crate::emit_result::EmitResult;

lazy_static! {
    pub static ref CONSTANTS: HashMap<String, f64> = HashMap::from([
        ("e".to_string(), f64::consts::E),
        ("pi".to_string(), f64::consts::PI),
        ("ln2".to_string(), f64::consts::LN_2),
        ("ln10".to_string(), f64::consts::LN_10),
        ("sqrt2".to_string(), f64::consts::SQRT_2),
    ]);

    pub static ref CONSTANTS_NAMES: HashMap<String, String> = HashMap::from([
        ("e".to_string(), "Math.E".to_string()),
        ("pi".to_string(), "Math.PI".to_string()),
        ("ln2".to_string(), "Math.LN2".to_string()),
        ("ln10".to_string(), "Math.LN10".to_string()),
        ("sqrt2".to_string(), "Math.SQRT2".to_string()),
    ]);
}

#[wasm_bindgen]
pub fn emit_bool_expr(
    expr: &str,
    equality_approximate_threshold: f64
) -> String {
    diagnostic::Diagnostic::clear();
    
    let token_iter = lexer::token_iter(expr);

    let ast = parser::parse_top_level_expression(
        parser::ParserContext::new(
            Box::new(token_iter),
            parser::create_binary_op_precedence()
        )
    );

    let mut result = String::new();

    if let Ok(ast) = ast {
        if !validator::validate_bool_equation(
            &ast, 
            &CONSTANTS,
            &HashMap::new(),
            &HashSet::from([
                "x".to_string(),
                "y".to_string()
            ])
        ) {
            result.push_str("Invalid expression");
        } else {
            result.push_str(transpiler::transplie_to_js(
                &ast,
                &CONSTANTS_NAMES,
                equality_approximate_threshold
            ).as_str());
        }
    } else {
        result.push_str("Invalid equation");
    }

    let serialized = serde_json::to_string(
        &EmitResult {
            code: result,
            diagnostics: diagnostic::Diagnostic::diagnostics().to_vec()
        }
    ).unwrap();
    
    serialized
}

#[wasm_bindgen]
pub fn emit_number_expr(
    expr: &str
) -> String {
    diagnostic::Diagnostic::clear();
    
    let token_iter = lexer::token_iter(expr);

    let ast = parser::parse_top_level_expression(
        parser::ParserContext::new(
            Box::new(token_iter),
            parser::create_binary_op_precedence()
        )
    );

    let mut result = String::new();

    if let Ok(ast) = ast {
        if !validator::validate_number_equation(
            &ast, 
            &CONSTANTS,
            &HashMap::new(),
            &HashSet::from([
                "x".to_string()
            ])
        ) {
            result.push_str("Invalid equation");
        } else {
            result.push_str(transpiler::transplie_to_js(
                &ast,
                &CONSTANTS_NAMES,
                0.0
            ).as_str());
        }
    } else {
        result.push_str("Invalid equation");
    }

    let serialized = serde_json::to_string(
        &EmitResult {
            code: result,
            diagnostics: diagnostic::Diagnostic::diagnostics().to_vec()
        }
    ).unwrap();

    serialized
}
