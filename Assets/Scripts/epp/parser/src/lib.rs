mod parser_context;

pub use parser_context::*;

use ast::Expr;
use lexer::Token;
use diagnostic::{Diagnostic, Level};

/// number_expr ::= number
fn parse_number_expr(ctx: &mut ParserContext, number: String) -> Box<Expr> {
    ctx.next_token();
    return Box::new(Expr::Literal(number.parse::<f64>().unwrap()));
}

/// paren_expr ::= '(' expression ')'
fn parse_paren_expr(ctx: &mut ParserContext) -> Result<Box<Expr>, ()> {
    ctx.next_token(); // eat (.
    let v = parse_expression(ctx)?;

    if let Some(Token::CloseParen) = ctx.current_token() {    
        ctx.next_token(); // eat ).
        Ok(v)
    } else {
        Diagnostic::push_new(Diagnostic::new(
            Level::Error,
            "expected ')'".to_string(),
        ));
        Err(())
    }
}

/// identifier_expr
///   ::= identifier
///   ::= identifier '(' expression* ')'
fn parse_identifier_expr(ctx: &mut ParserContext, id_name: String) -> Result<Box<Expr>, ()> {
    ctx.next_token(); // eat identifier.

    match ctx.current_token() {
        Some(&Token::OpenParen) => {
            // Call.
            ctx.next_token(); // eat (
            let mut args = Vec::new();
            
            if ctx.current_token().is_none() || ctx.current_token().unwrap() != &Token::CloseParen {
                loop {
                    let arg = parse_expression(ctx)?;
                    args.push(arg);
                    
                    if let Some(Token::CloseParen) = ctx.current_token() {
                        break;
                    } else if ctx.current_token().map_or(true, |tok| tok != &Token::Comma) {
                        Diagnostic::push_new(Diagnostic::new(
                            Level::Error,
                            "Expected ')' or ',' in argument list".to_string(),
                        ));
                        return Err(());
                    }

                    ctx.next_token();
                }
            }
    
            // Eat the ')'.
            ctx.next_token();
        
            return Ok(Box::new(Expr::Call(id_name.to_owned(), args)));
        },
        _ => { // Simple variable ref.
            return Ok(Box::new(Expr::Id(id_name)));
        }
    }
}

/// primary
///   ::= identifier_expr
///   ::= number_expr
///   ::= paren_expr
fn parse_primary(ctx: &mut ParserContext, unary_check: bool) -> Result<Box<Expr>, ()> {
    match ctx.current_token() {
        Some(token) => {
            match token {
                Token::Id(id_name) => {
                    let id_name = id_name.to_owned();
                    parse_identifier_expr(ctx, id_name)
                },
                Token::NumberLiteral(number) => {
                    let number = number.to_owned();
                    Ok(parse_number_expr(ctx, number))
                },
                Token::Minus => {
                    if !unary_check {
                        Diagnostic::push_new(Diagnostic::new(
                            Level::Error,
                            "Unexpected '-'".to_string(),
                        ));
                        return Err(());
                    }

                    ctx.next_token();
                    
                    match parse_primary(ctx, false) {
                        Ok(expr) => Ok(Box::new(Expr::Unary(expr))),
                        Err(_) => {
                            Diagnostic::push_new(Diagnostic::new(
                                Level::Error,
                                "Expected number, identifier, or '(' after '-'".to_string(),
                            ));
                            Err(())
                        },
                    }
                },
                Token::OpenParen => parse_paren_expr(ctx),
                _ => {
                    Diagnostic::push_new(Diagnostic::new(
                        Level::Error,
                        "unexpected token when parsing primary".to_string(),
                    ));
                    Err(())
                },
            }
        },
        None => {
            Diagnostic::push_new(Diagnostic::new(
                Level::Error,
                "unexpected end of input when parsing primary".to_string(),
            ));
            Err(())
        }
    }
}

/// bin_op_rhs
///   ::= ('+' primary)*
fn parse_bin_op_rhs(ctx: &mut ParserContext, expr_precedence: i32, mut lhs: Box<Expr>) -> Result<Box<Expr>, ()> {
    // If this is a bin_op, find its precedence.
    loop {
	    let tok_precedence = ctx.current_token().map_or(-1, |tok| ctx.get_token_precedence(tok));
        
        // If this is a bin_op that binds at least as tightly as the current bin_op,
        // consume it, otherwise we are done.
        if tok_precedence < expr_precedence {
            return Ok(lhs);
        }

        // Okay, we know this is a bin_op.
        let bin_op = ctx.current_token().unwrap().to_owned();
        ctx.next_token(); // eat bin_op

        // Parse the primary expression after the binary operator.
        let mut rhs = parse_primary(ctx, true)?;

        // If BinOp binds less tightly with RHS than the operator after RHS, let
        // the pending operator take RHS as its LHS.
        let next_precedence = ctx.current_token().map_or(-1, |token| ctx.get_token_precedence(token));
        
        if tok_precedence < next_precedence {
            rhs = parse_bin_op_rhs(ctx, tok_precedence + 1, rhs)?;
        }

        // Merge LHS/RHS.
        lhs = Box::new(
            
            match bin_op {
                Token::Eq => Expr::Eq(lhs, rhs),
                Token::Lt => Expr::Lt(lhs, rhs),
                Token::Gt => Expr::Gt(lhs, rhs),
                Token::Le => Expr::Le(lhs, rhs),
                Token::Ge => Expr::Ge(lhs, rhs),
                Token::Plus => Expr::Add(lhs, rhs),
                Token::Minus => Expr::Sub(lhs, rhs),
                Token::Star => Expr::Mul(lhs, rhs),
                Token::Slash => Expr::Div(lhs, rhs),
                Token::Percent => Expr::Mod(lhs, rhs),
                Token::Caret => Expr::Pow(lhs, rhs),
                _ => unreachable!(),
            }
        );
    }
}

/// expression
///   ::= primary bin_op_rhs
///
fn parse_expression(ctx: &mut ParserContext) -> Result<Box<Expr>, ()> {
    let lhs = parse_primary(ctx, true)?;
    return parse_bin_op_rhs(ctx, 0, lhs);
}

pub fn parse_top_level_expression(mut ctx: ParserContext) -> Result<Box<Expr>, ()> {
    ctx.next_token();
    let result = parse_expression(&mut ctx);

    if ctx.current_token().is_some() {
        Diagnostic::push_new(Diagnostic::new(
            Level::Error,
            "unexpected token after top-level expression".to_string(),
        ));
        Err(())
    } else {
        result
    }
}
