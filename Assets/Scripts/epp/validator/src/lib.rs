use std::collections::{HashMap, HashSet};

use diagnostic::{Diagnostic, Level};
use ast::Expr;

#[macro_use]
extern crate lazy_static;

lazy_static! {
    static ref FUNCTION_MAP: HashMap<&'static str, usize> = {
        let mut map = HashMap::new();
        //function_name, parameter_count
        map.insert("abs", 1);
        map.insert("acos", 1);
        map.insert("acosh", 1);
        map.insert("asin", 1);
        map.insert("asinh", 1);
        map.insert("atan", 1);
        map.insert("atan2", 2);
        map.insert("atanh", 1);
        map.insert("cbrt", 1);
        map.insert("ceil", 1);
        map.insert("cos", 1);
        map.insert("cosh", 1);
        map.insert("exp", 1);
        map.insert("exp_m1", 1);
        map.insert("floor", 1);
        map.insert("hypot", 2);
        map.insert("ln", 1);
        map.insert("ln_1p", 1);
        map.insert("log", 2);
        map.insert("log10", 1);
        map.insert("log2", 1);
        map.insert("max", 2);
        map.insert("min", 2);
        map.insert("pow", 2);
        map.insert("round", 1);
        map.insert("sin", 1);
        map.insert("sinh", 1);
        map.insert("sqrt", 1);
        map.insert("tan", 1);
        map.insert("tanh", 1);
        map
    };
}

fn validate_equation(
    ast: &Box<Expr>,
    constants: &HashMap<String, f64>,
    variables: &HashMap<String, f64>,
    un_evaluated_variables: &HashSet<String>,
    relational_operation_count: usize,
) -> bool {
    let id_table = make_id_list(ast);
    let expr_count_map = count_expr_count(ast);

    let mut var_set = variables.keys().chain(un_evaluated_variables.iter()).cloned().collect::<HashSet<_>>();
    
    let ids = id_table.ids;
    for name in ids {
        if var_set.contains(&name) {
            var_set.remove(&name);
        } else if !constants.contains_key(&name) {
            Diagnostic::push_new(Diagnostic::new(
                Level::Error,
                format!("Variable or Constant {} is not defined", name),
            ));
            return false;
        }
    }

    for var_name in var_set {
        Diagnostic::push_new(Diagnostic::new(
            Level::Warning,
            format!("Variable {} is not used", var_name),
        ));
    }

    let functions = id_table.called_ids;
    for function in functions {
        if !FUNCTION_MAP.contains_key(function.as_str()) {
            Diagnostic::push_new(Diagnostic::new(
                Level::Error,
                format!("Function {} is not defined", function),
            ));
            return false;
        }
    }

    let mut function_call_argument_error = false;
    
    traverse_ast(
        ast,
        &mut |expr| {
            if let Expr::Call(name, args) = expr.as_ref() {
                if let Some(param_count) = FUNCTION_MAP.get(name.as_str()) {
                    if args.len() != param_count.to_owned() {
                        Diagnostic::push_new(Diagnostic::new(
                            Level::Error,
                            format!("Function '{}' takes {} arguments", name, param_count),
                        ));
                        function_call_argument_error = true;
                    }
                }
            }
        },
    );

    if function_call_argument_error {
        return false;
    }

    let mut relation_expr_count = 0;

    for (expr, count) in expr_count_map {
        if expr == Expr::eq_str() ||
            expr == Expr::lt_str() ||
            expr == Expr::gt_str() ||
            expr == Expr::le_str() ||
            expr == Expr::ge_str()
        {
            relation_expr_count += count;
        }
    }

    if relational_operation_count != relation_expr_count as usize {
        Diagnostic::push_new(Diagnostic::new(
            Level::Error,
            format!("relation expression must be used once"),
        ));
        return false;
    }

    true
}

pub fn validate_number_equation(
    ast: &Box<Expr>,
    constants: &HashMap<String, f64>,
    variables: &HashMap<String, f64>,
    un_evaluated_variables: &HashSet<String>
) -> bool {
    validate_equation(
        ast,
        constants,
        variables,
        un_evaluated_variables,
        0,
    )
}

pub fn validate_bool_equation(
    ast: &Box<Expr>,
    constants: &HashMap<String, f64>,
    variables: &HashMap<String, f64>,
    un_evaluated_variables: &HashSet<String>
) -> bool {
    validate_equation(
        ast,
        constants,
        variables,
        un_evaluated_variables,
        1,
    )
}

#[derive(Debug, Clone)]
struct IdTable {
    pub(crate) ids: HashSet<String>,
    pub(crate) called_ids: HashSet<String>,
}

fn make_id_list(ast: &Box<Expr>) -> IdTable {
    let mut result = IdTable { ids: HashSet::new(), called_ids: HashSet::new() };

    traverse_ast(
        &ast, 
        &mut |ast| {
            match &**ast {
                Expr::Id(id) => {
                    result.ids.insert(id.to_owned());
                },
                Expr::Call(id, _) => {
                    result.called_ids.insert(id.to_owned());
                }
                _ => { }
            }
        }
    );

    result
}

fn count_expr_count(ast: &Box<Expr>) -> HashMap<&'static str, i32> {
    let mut result = HashMap::new();

    traverse_ast(
        &ast, 
        &mut |ast| {
            result.entry(ast.to_str()).and_modify(|e| *e += 1).or_insert(1);
        }
    );

    result
}

fn traverse_ast(ast: &Box<Expr>, func: &mut impl FnMut(&Box<Expr>)) {
    match &**ast {
        Expr::Id(_) => {
            func(ast);
        },
        Expr::Call(_, args) => {
            func(ast);
            for arg in args {
                traverse_ast(&arg, func);
            }
        },
        Expr::Eq(lhs, rhs)
        | Expr::Lt(lhs, rhs)
        | Expr::Gt(lhs, rhs)
        | Expr::Le(lhs, rhs)
        | Expr::Ge(lhs, rhs)
        | Expr::Add(lhs, rhs)
        | Expr::Sub(lhs, rhs)
        | Expr::Mul(lhs, rhs)
        | Expr::Div(lhs, rhs)
        | Expr::Mod(lhs, rhs)
        | Expr::Pow(lhs, rhs) => {
            func(ast);
            traverse_ast(&lhs, func);
            traverse_ast(&rhs, func);
        },
        Expr::Unary(expr) => {
            func(ast);
            traverse_ast(&expr, func);
        },
        Expr::Literal(_) => {
            func(ast);
        }
    }
}
