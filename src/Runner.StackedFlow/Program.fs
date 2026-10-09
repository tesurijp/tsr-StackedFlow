open tsr.StackedFlow

let add x y z = x + y + z

let x = 
    EmptyStack
    |> Stack.push 11
    |> Stack.dup
    |> Stack.push 10
    |> Stack.printStack
    |> Stack.dup
    |> Stack.printStack
    |> FuncApplyer.apply3 add
    |> Stack.printStack
    |> FuncApplyer.apply2 (*)
    |> Stack.printStack

open tsr.StackedFlow.Stack

EmptyStack
|> push 10
|> push 10
|> push "string"
|> printStack
|> swap
|> printStack
|> push "11"
|> dup
|> push "string"
|> printStack
|> ignore

open tsr.StackedFlow.FuncApplyer
open tsr.StackedFlow.StackOperator

EmptyStack
|+ 10
|+ 10
|> dup
|+ 10
|> dup
|!!! add
|> printStack
|!! (*)
|> printStack
|> ignore

EmptyStack
|+ 10
|+ 12
|+ 10
|!!! add
|> printStack
|> ignore

EmptyStack
|+ 10
|+ 10
|+ 10
|+ 10
|> printStack
|> apply2 (+)
|> printStack
|!! (+)
|> printStack
|> ignore

EmptyStack
|+ 1
|+ 2
|+ 3
|> printStack
|> rot
|> printStack
|> ignore


EmptyStack
|+ 1
|+ 2
|> printStack
|> over
|> printStack
|> ignore

EmptyStack
    |+ 1
    |+ 2
    |+ 3
    |+ 4
    |+ 5
    |+ 6
    |+ 7
    |+ "abc"
    |+ "xyz"
    |> printStack
    |> Caller.prepareArgs5
    |> Caller.moreArgs2
    |> Caller.moreArgs2
    |> Caller.setFunc (fun a b c d e f g h i -> $"{a - b} + {c} + {d} + {e} + {f} {g} {h} {i}")
    |> Caller.callArgs5
    |> Caller.callArgs2
    |> Caller.callArgs2
    |> Caller.finalize
    |> printStack
    |> ignore

EmptyStack
    |+ "B+C "
    |+ 2
    |+ 3
    |+ "E + F - G "
    |+ 5
    |+ 6
    |+ 7
    |> printStack
    |> Caller.prepareArgs5
    |> Caller.moreArgs2
    |> Caller.setFunc (fun a b c d e f g -> $"{a} = {b + c}  {d} = {e + f - g}" )
    |> Caller.callArgs5
    |> Caller.callArgs2
    |> Caller.finalize
    |> head
    |> printf "%s"
    |> ignore


