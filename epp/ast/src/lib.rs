#[derive(Debug, Clone)]
pub enum Expr {
    Eq(Box<Expr>, Box<Expr>),
    Lt(Box<Expr>, Box<Expr>),
    Gt(Box<Expr>, Box<Expr>),
    Le(Box<Expr>, Box<Expr>),
    Ge(Box<Expr>, Box<Expr>),
    Unary(Box<Expr>),
    Add(Box<Expr>, Box<Expr>),
    Sub(Box<Expr>, Box<Expr>),
    Mul(Box<Expr>, Box<Expr>),
    Div(Box<Expr>, Box<Expr>),
    Mod(Box<Expr>, Box<Expr>),
    Pow(Box<Expr>, Box<Expr>),
    Call(String, Vec<Box<Expr>>),
    Id(String),
    Literal(f64),
}

impl Expr {
    pub fn to_str(&self) -> &'static str {
        match self {
            Expr::Eq(..) => Expr::eq_str(),
            Expr::Lt(..) => Expr::lt_str(),
            Expr::Gt(..) => Expr::gt_str(),
            Expr::Le(..) => Expr::le_str(),
            Expr::Ge(..) => Expr::ge_str(),
            Expr::Unary(..) => Expr::unary_str(),
            Expr::Add(..) => Expr::add_str(),
            Expr::Sub(..) => Expr::sub_str(),
            Expr::Mul(..) => Expr::mul_str(),
            Expr::Div(..) => Expr::div_str(),
            Expr::Mod(..) => Expr::mod_str(),
            Expr::Pow(..) => Expr::pow_str(),
            Expr::Call(..) => Expr::call_str(),
            Expr::Id(..) => Expr::id_str(),
            Expr::Literal(..) => Expr::literal_str(),
        }
    }

    pub const fn eq_str() -> &'static str {
        "="
    }

    pub fn lt_str() -> &'static str {
        "<"
    }

    pub fn gt_str() -> &'static str {
        ">"
    }

    pub fn le_str() -> &'static str {
        "<="
    }

    pub fn ge_str() -> &'static str {
        ">="
    }

    pub fn unary_str() -> &'static str {
        "unary"
    }

    pub fn add_str() -> &'static str {
        "+"
    }

    pub fn sub_str() -> &'static str {
        "-"
    }

    pub fn mul_str() -> &'static str {
        "*"
    }

    pub fn div_str() -> &'static str {
        "/"
    }

    pub fn mod_str() -> &'static str {
        "%"
    }

    pub fn pow_str() -> &'static str {
        "^"
    }

    pub fn call_str() -> &'static str {
        "call"
    }

    pub fn id_str() -> &'static str {
        "id"
    }

    pub fn literal_str() -> &'static str {
        "literal"
    }
}
