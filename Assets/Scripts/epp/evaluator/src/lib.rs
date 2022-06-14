use std::collections::HashMap;
use ast::Expr;

#[derive(Debug)]
pub struct EvalResult {
    lhs: f64,
    op: &'static str,
    rhs: f64,
    eval_result: bool,
}

impl EvalResult {
    pub fn lhs(&self) -> f64 {
        self.lhs
    }

    pub fn op(&self) -> &'static str {
        self.op
    }

    pub fn rhs(&self) -> f64 {
        self.rhs
    }

    pub fn eval_result(&self) -> bool {
        self.eval_result
    }
}

pub fn eval_equation(
    ast: &Box<Expr>,
    variables: &HashMap<String, f64>,
    equality_approximate_threshold: f64,
) -> Result<EvalResult, ()> {
    match ast.as_ref() {
        Expr::Eq(lhs, rhs) => {
            let lhs = fold_const_expr(lhs, variables);
            let rhs = fold_const_expr(rhs, variables);
            return Ok(
                EvalResult {
                    rhs, op: ast.to_str(), lhs,
                    eval_result: f64::abs(lhs - rhs) < equality_approximate_threshold
                }
            );
        },
        Expr::Lt(lhs, rhs) => {
            let lhs = fold_const_expr(lhs, variables);
            let rhs = fold_const_expr(rhs, variables);
            return Ok(
                EvalResult {
                    rhs, op: ast.to_str(), lhs,
                    eval_result: lhs < rhs
                }
            );
        },
        Expr::Gt(lhs, rhs) => {
            let lhs = fold_const_expr(lhs, variables);
            let rhs = fold_const_expr(rhs, variables);
            return Ok(
                EvalResult {
                    rhs, op: ast.to_str(), lhs,
                    eval_result: lhs > rhs
                }
            );
        },
        Expr::Le(lhs, rhs) => {
            let lhs = fold_const_expr(lhs, variables);
            let rhs = fold_const_expr(rhs, variables);
            return Ok(
                EvalResult {
                    rhs, op: ast.to_str(), lhs,
                    eval_result: lhs <= rhs
                }
            );
        },
        Expr::Ge(lhs, rhs) => {
            let lhs = fold_const_expr(lhs, variables);
            let rhs = fold_const_expr(rhs, variables);
            return Ok(
                EvalResult {
                    rhs, op: ast.to_str(), lhs,
                    eval_result: lhs >= rhs
                }
            );
        },
        _ => unreachable!(),
    }
}

fn fold_const_expr(ast: &Box<Expr>, variables: &HashMap<String, f64>) -> f64 {
    match ast.as_ref() {
        Expr::Literal(value) => value.clone(),
        Expr::Add(lhs, rhs) => fold_const_expr(lhs, variables) + fold_const_expr(rhs, variables),
        Expr::Sub(lhs, rhs) => fold_const_expr(lhs, variables) - fold_const_expr(rhs, variables),
        Expr::Mul(lhs, rhs) => fold_const_expr(lhs, variables) * fold_const_expr(rhs, variables),
        Expr::Div(lhs, rhs) => fold_const_expr(lhs, variables) / fold_const_expr(rhs, variables),
        Expr::Mod(lhs, rhs) => fold_const_expr(lhs, variables) % fold_const_expr(rhs, variables),
        Expr::Pow(lhs, rhs) => fold_const_expr(lhs, variables).powf(fold_const_expr(rhs, variables)),
        Expr::Unary(expr) => -fold_const_expr(expr, variables),
        Expr::Id(id) => {
            if let Some(value) = variables.get(id) {
                value.clone()
            } else {
                panic!("variable not found");
            }
        },
        Expr::Eq(..)
        | Expr::Lt(..)
        | Expr::Gt(..)
        | Expr::Le(..)
        | Expr::Ge(..) => panic!("constant expression expected"),
        Expr::Call(func_name, params) => {
            let params = params.iter().map(|param| fold_const_expr(param, variables)).collect::<Vec<f64>>();
            match func_name.as_str() {
                "abs" => f64::abs(params[0]),
                "acos" => f64::acos(params[0]),
                "acosh" => f64::acosh(params[0]),
                "asin" => f64::asin(params[0]),
                "asinh" => f64::asinh(params[0]),
                "atan" => f64::atan(params[0]),
                "atan2" => f64::atan2(params[0], params[1]),
                "atanh" => f64::atanh(params[0]),
                "cbrt" => f64::cbrt(params[0]),
                "ceil" => f64::ceil(params[0]),
                "cos" => f64::cos(params[0]),
                "cosh" => f64::cosh(params[0]),
                "exp" => f64::exp(params[0]),
                "exp_m1" => f64::exp_m1(params[0]),
                "floor" => f64::floor(params[0]),
                "hypot" => f64::hypot(params[0], params[1]),
                "ln" => f64::ln(params[0]),
                "ln_1p" => f64::ln_1p(params[0]),
                "log" => f64::log(params[0], params[1]),
                "log10" => f64::log10(params[0]),
                "log2" => f64::log2(params[0]),
                "max" => f64::max(params[0], params[1]),
                "min" => f64::min(params[0], params[1]),
                "pow" => f64::powf(params[0], params[1]),
                "round" => f64::round(params[0]),
                "sin" => f64::sin(params[0]),
                "sinh" => f64::sinh(params[0]),
                "sqrt" => f64::sqrt(params[0]),
                "tan" => f64::tan(params[0]),
                "tanh" => f64::tanh(params[0]),
                _ => panic!("function not found"),
            }
        }
    }
}
