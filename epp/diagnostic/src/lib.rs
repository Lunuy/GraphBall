#[cfg(feature = "global_instance")]
pub(crate) mod global_instance;
mod level;

pub use level::*;

#[cfg(feature = "global_instance")]
use crate::global_instance::DIAGNOSTICS;
#[cfg(feature = "global_instance")]
use parking_lot::MutexGuard;
use serde::Serialize;

#[derive(Debug, Clone, Hash, Serialize)]
pub struct Diagnostic {
    level: Level,
    message: String,
}

impl Diagnostic {
    pub fn new(level: Level, message: String) -> Self {
        Self {
            level,
            message,
        }
    }

    pub fn level(&self) -> Level {
        self.level
    }

    pub fn message(&self) -> &str {
        &self.message
    }

    #[cfg(feature = "global_instance")]
    pub fn push_new(diagnostic: Diagnostic) {
        DIAGNOSTICS.lock().push(diagnostic);
    }

    #[cfg(feature = "global_instance")]
    pub fn diagnostics() -> MutexGuard<'static, Vec<Diagnostic>> {
        DIAGNOSTICS.lock()
    }

    #[cfg(feature = "global_instance")]
    pub fn clear() {
        DIAGNOSTICS.lock().clear();
    }
}
