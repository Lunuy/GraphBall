use serde::Serialize;
use diagnostic::Diagnostic;

#[derive(Serialize)]
pub(crate) struct EmitResult {
    pub(crate) code: String,
    pub(crate) diagnostics: Vec<Diagnostic>,
}
