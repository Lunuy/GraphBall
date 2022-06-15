use std::collections::{HashMap, HashSet};

use lexer;
use parser;
use diagnostic;
use evaluator;
use transpiler;
use validator;
use std::f64;

fn main() {
    let mut input = String::new();
    std::io::stdin().read_line(&mut input).unwrap();

    let token_iter = lexer::token_iter(&input);

    let ast = parser::parse_top_level_expression(
        parser::ParserContext::new(
            Box::new(token_iter),
            parser::create_binary_op_precedence()
        )
    );

    println!("{:?}", ast);

    if let Ok(ast) = ast {
        let variables = HashMap::from([
            ("x".to_string(), 1.0),
            ("y".to_string(), 2.0),
            ("z".to_string(), 3.0),
        ]);

        let constants = HashMap::from([
            ("e".to_string(), f64::consts::E),
            ("pi".to_string(), f64::consts::PI),
            ("ln2".to_string(), f64::consts::LN_2),
            ("ln10".to_string(), f64::consts::LN_10),
            ("sqrt2".to_string(), f64::consts::SQRT_2),
        ]);

        if validator::validate_bool_equation(
            &ast, &constants, &variables,
            &HashSet::new()
        ) {
            let eval_result = evaluator::eval_equation(
                &ast,
                &variables.into_iter().chain(constants.into_iter()).collect(),
                0.001
            );

            println!("{:?}", eval_result);

            println!("{}", transpiler::transplie_to_js(
                &ast,
                &HashMap::new(),
                0.001
            ));
        }
    }

    println!("{:?}", diagnostic::Diagnostic::diagnostics());
}
