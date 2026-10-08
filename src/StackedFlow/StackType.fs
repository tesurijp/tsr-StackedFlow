
namespace tsr.StackedFlow

type IStack = interface end

type EmptyStack =
    | EmptyStack
    override _.ToString() = null
    interface IStack

type Stack<'head, 'tail when 'tail :> IStack> =
    | Stack of 'head * 'tail
    override this.ToString() =
        match this with Stack(head, tail) -> 
            match tail.ToString() with
            | null -> $"{head}"
            | _ -> $"{head}::{tail}"
    interface IStack
