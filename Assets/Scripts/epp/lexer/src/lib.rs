mod cursor;
mod token;

pub use token::*;

use cursor::*;
use std::iter::from_fn as iter_from_fn;
use unicode_xid::UnicodeXID;

pub fn token_iter(mut input: &str) -> impl Iterator<Item = Token> + '_ {
    iter_from_fn(move || {
        let mut cursor = Cursor::new(input);
        consume_whitespace(&mut cursor);
        input = &input[cursor.len_consumed()..];

        if input.is_empty() {
            return None;
        }

        let (token, consume_len) = next(input);
        input = &input[consume_len..];
        Some(token)
    })
}

fn next(input: &str) -> (Token, usize) {
    let mut cursor = Cursor::new(input);

    let kind = match cursor.consume().unwrap() {
        char if is_id_start(char) => {
            consume_while(&mut cursor, |char| is_id_continue(char));
            Token::Id(input[..cursor.len_consumed()].to_string())
        }
        '0'..='9' => {
            consume_number(&mut cursor);
            let suffix_start = cursor.len_consumed();

            Token::NumberLiteral(input[..suffix_start].to_string())
        }
        '(' => Token::OpenParen,
        ')' => Token::CloseParen,
        ',' => Token::Comma,
        '=' => Token::Eq,
        '<' => {
            consume_whitespace(&mut cursor);
            if cursor.lookup(0) == '=' {
                cursor.consume();
                Token::Le
            } else {
                Token::Lt
            }
        },
        '>' => {
            consume_while(&mut cursor, |char| char.is_whitespace());
            if cursor.lookup(0) == '=' {
                cursor.consume();
                Token::Ge
            } else {
                Token::Gt
            }
        },
        '+' => Token::Plus,
        '-' => Token::Minus,
        '*' => Token::Star,
        '/' => Token::Slash,
        '%' => Token::Percent,
        '^' => Token::Caret,
        _ => Token::Unknown,
    };

    (kind, cursor.len_consumed())
}

fn consume_while(cursor: &mut Cursor, mut pred: impl FnMut(char) -> bool) {
    while pred(cursor.first()) {
        cursor.consume();
    }
}

fn consume_whitespace(cursor: &mut Cursor) {
    consume_while(cursor, |char| char.is_whitespace());
}

fn is_id_start(char: char) -> bool {
    ('a'..='z').contains(&char)
        || ('A'..='Z').contains(&char)
        || (char == '_')
        || (char > '\x7f' && UnicodeXID::is_xid_start(char))
}

fn is_id_continue(char: char) -> bool {
    ('a'..='z').contains(&char)
        || ('A'..='Z').contains(&char)
        || ('0'..='9').contains(&char)
        || (char == '_')
        || (char > '\x7f' && UnicodeXID::is_xid_continue(char))
}

fn consume_number(cursor: &mut Cursor) {
    match cursor.first() {
        '.' if cursor.second().is_digit(10) => {
            cursor.consume();
            consume_while(cursor, |char| char.is_digit(10));
        }
        '0'..='9' => {
            cursor.consume();
            consume_while(cursor, |char| char.is_digit(10));

            if cursor.first() == '.' {
                cursor.consume();
                consume_while(cursor, |char| char.is_digit(10));
            }
        }
        _ => {
            consume_while(cursor, |char| char.is_digit(10));
            return;
        }
    }
}
