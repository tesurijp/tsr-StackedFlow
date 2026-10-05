
namespace tsr.StackedFlow

type IStack = 
    abstract member ToStrings: unit -> string list

type EmptyStack =
    | EmptyStack
    interface IStack with
        member _.ToStrings() = []

type Stack<'head , 'tail when 'tail:> IStack> =
    | Stack of 'head * 'tail
    interface IStack with
        member this.ToStrings (): string list=
            let (Stack (head, tail)) = this
            sprintf "%A" head ::  tail.ToStrings()

[<AutoOpen>]
module Functions =

    let printStack (stack) =
        (stack :> IStack).ToStrings()
        |> String.concat " :: "
        |> printfn "%s"
        stack

    let push x stack = Stack(x, stack)
    let dup stack =
        let (Stack(head, _)) = stack
        Stack(head, stack)
    let drop stack =
        let (Stack(_, tail)) = stack
        tail
    let swap stack =
        let (Stack(h1, Stack(h2, tail))) = stack
        Stack(h2, Stack(h1, tail))
    let pop stack =
        let (Stack(head, tail)) = stack
        head, tail
    let head stack =
        let (Stack(head, tail)) = stack
        head


    let apply1 (func) (stack) =
        let (Stack(a1:'a1, tail)) = stack
        Stack(func a1, tail)

    let apply2 (func) (stack) =
        let (Stack(a1, Stack(a2, tail))) = stack
        Stack(func a1 a2 , tail)

    let apply3 (func) (stack) =
        let (Stack(a1, Stack(a2, Stack(a3,  tail)))) = stack
        Stack(func a1 a2 a3 , tail)

    let apply4 (func) (stack) =
        let (Stack(a1, Stack(a2, Stack(a3,  Stack(a4,  tail))))) = stack
        Stack(func a1 a2 a3 a4 , tail)

    let (|+) stack value = push value stack
    let (|!) stack func = apply1 func stack
    let (|!!) stack func = apply2 func stack
    let (|!!!) stack func = apply3 func stack
    let (|!!!!) stack func = apply4 func stack

