#[derive(Debug, Clone, Copy, PartialEq, Eq, PartialOrd, Ord, Hash)]
#[repr(C)]
pub enum Level {
    Error = 0,
    Warning = 1,
    Note = 2,
}
