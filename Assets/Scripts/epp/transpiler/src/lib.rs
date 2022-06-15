use std::collections::HashMap;

use ast::Expr;

#[macro_use]
extern crate lazy_static;

lazy_static! {
    static ref JS_FUNCTION_MAP: HashMap<&'static str, &'static str> = {
        let mut map = HashMap::new();
        //function_name => js_function_name
        map.insert("abs", "Math.abs");
        map.insert("acos", "Math.acos");
        map.insert("acosh", "Math.acosh");
        map.insert("asin", "Math.asin");
        map.insert("asinh", "Math.asinh");
        map.insert("atan", "Math.atan");
        map.insert("atan2", "Math.atan2");
        map.insert("atanh", "Math.atanh");
        map.insert("cbrt", "Math.cbrt");
        map.insert("ceil", "Math.ceil");
        map.insert("cos", "Math.cos");
        map.insert("cosh", "Math.cosh");
        map.insert("exp", "Math.exp");
        map.insert("exp_m1", "Math.expm1");
        map.insert("floor", "Math.floor");
        map.insert("hypot", "Math.hypot");
        map.insert("ln", "Math.log");
        map.insert("ln_1p", "Math.log1p");
        map.insert("log", "[this function needs custom implementation]");
        map.insert("log10", "Math.log10");
        map.insert("log2", "Math.log2");
        map.insert("max", "Math.max");
        map.insert("min", "Math.min");
        map.insert("pow", "Math.pow");
        map.insert("round", "Math.round");
        map.insert("sin", "Math.sin");
        map.insert("sinh", "Math.sinh");
        map.insert("sqrt", "Math.sqrt");
        map.insert("tan", "Math.tan");
        map.insert("tanh", "Math.tanh");
        map
    };
}

pub fn transplie_to_js(
    ast: &Box<Expr>,
    constant_name_map: &HashMap<String, String>,
    equality_approximate_threshold: f64,
) -> String {
    let mut result = String::new();

    transplie_to_js_internal(ast, constant_name_map, equality_approximate_threshold, &mut result);
    result
}

fn transplie_to_js_internal(
    ast: &Box<Expr>,
    constant_name_map: &HashMap<String, String>,
    equality_approximate_threshold: f64,
    result: &mut String, 
) {
    match ast.as_ref() {
        Expr::Id(id) => {
            if let Some(constant_name) = constant_name_map.get(id) {
                result.push_str(constant_name);
            } else {
                result.push_str(id);
            }
        },
        Expr::Call(id, args) => {
            if id == "log" {
                result.push('(');

                result.push_str("Math.log2(");
                transplie_to_js_internal(
                    &args[0],
                    constant_name_map,
                    equality_approximate_threshold,
                    result
                );
                result.push(')');
                result.push_str(" / ");
                result.push_str("Math.log2(");
                transplie_to_js_internal(
                    &args[1],
                    constant_name_map,
                    equality_approximate_threshold,
                    result
                );
                result.push(')');

                result.push(')');
            } else {
                result.push_str(JS_FUNCTION_MAP.get(id.as_str()).expect("function translation not found"));
                result.push('(');
                for arg in args {
                    transplie_to_js_internal(
                        arg,
                        constant_name_map,
                        equality_approximate_threshold,
                        result
                    );
                    result.push_str(", ");
                }
                result.pop();
                result.pop();
                result.push(')');
            }
        },
        Expr::Eq(lhs, rhs) => {
            result.push('(');
            result.push_str("Math.abs(");
            transplie_to_js_internal(
                lhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push_str(" - ");
            transplie_to_js_internal(
                rhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push(')');
            result.push_str(" < ");
            result.push_str(&equality_approximate_threshold.to_string());
            result.push(')');
        },
        Expr::Lt(lhs, rhs) => {
            result.push('(');
            transplie_to_js_internal(
                lhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push_str(" < ");
            transplie_to_js_internal(
                rhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push(')');
        },
        Expr::Gt(lhs, rhs) => {
            result.push('(');
            transplie_to_js_internal(
                lhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push_str(" > ");
            transplie_to_js_internal(
                rhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push(')');
        },
        Expr::Le(lhs, rhs) => {
            result.push('(');
            transplie_to_js_internal(
                lhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push_str(" <= ");
            transplie_to_js_internal(
                rhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push(')');
        },
        Expr::Ge(lhs, rhs) => {
            result.push('(');
            transplie_to_js_internal(
                lhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push_str(" >= ");
            transplie_to_js_internal(
                rhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push(')');
        },
        Expr::Add(lhs, rhs) => {
            result.push('(');
            transplie_to_js_internal(
                lhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push_str(" + ");
            transplie_to_js_internal(
                rhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push(')');
        },
        Expr::Sub(lhs, rhs) => {
            result.push('(');
            transplie_to_js_internal(
                lhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push_str(" - ");
            transplie_to_js_internal(
                rhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push(')');
        },
        Expr::Mul(lhs, rhs) => {
            result.push('(');
            transplie_to_js_internal(
                lhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push_str(" * ");
            transplie_to_js_internal(
                rhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push(')');
        },
        Expr::Div(lhs, rhs) => {
            result.push('(');
            transplie_to_js_internal(
                lhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push_str(" / ");
            transplie_to_js_internal(
                rhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push(')');
        },
        Expr::Mod(lhs, rhs) => {
            result.push('(');
            transplie_to_js_internal(
                lhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push_str(" % ");
            transplie_to_js_internal(
                rhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push(')');
        },
        Expr::Pow(lhs, rhs) => {
            result.push('(');
            transplie_to_js_internal(
                lhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push_str(" ** ");
            transplie_to_js_internal(
                rhs,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push(')');
        },
        Expr::Unary(expr) => {
            result.push('(');
            result.push('-');
            transplie_to_js_internal(
                expr,
                constant_name_map,
                equality_approximate_threshold,
                result
            );
            result.push(')');
        },
        Expr::Literal(literal) => {
            result.push_str(&literal.to_string());
        },
    }
}
