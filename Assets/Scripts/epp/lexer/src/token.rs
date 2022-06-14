#[derive(Debug, Clone, Eq, PartialEq, PartialOrd, Ord, Hash)]
pub enum Token {
    Unknown,
    OpenParen,    // "("
    CloseParen,   // ")"
    Comma,        // ","
    Eq,           // "="
    Lt,           // "<"
    Gt,           // ">"
    Le,           // "<="
    Ge,           // ">="
    Plus,         // "+"
    Minus,        // "-"
    Star,         // "*"
    Slash,        // "/"
    Percent,      // "%"
    Caret,        // "^"
    Id(String),   // identifier or keyword
    NumberLiteral(String),
}

impl Token {
    pub fn to_str(&self) -> &'static str {
        match self {
            Token::Unknown => "unknown",
            Token::OpenParen => "(",
            Token::CloseParen => ")",
            Token::Comma => ",",
            Token::Eq => "=",
            Token::Lt => "<",
            Token::Gt => ">",
            Token::Le => "<=",
            Token::Ge => ">=",
            Token::Plus => "+",
            Token::Minus => "-",
            Token::Star => "*",
            Token::Slash => "/",
            Token::Percent => "%",
            Token::Caret => "^",
            Token::Id(..) => "id",
            Token::NumberLiteral(..) => "literal",
        }
    }
}
