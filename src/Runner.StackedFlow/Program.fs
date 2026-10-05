open tsr.StackedFlow

let add x y z = x + y + z

let x = 
    EmptyStack
    |+ 11
    |> dup
    |+ 10
    |> printStack  // 
    |> dup
    |> printStack  // 
    |> apply3 add
    |> printStack  // 
    |> apply2 (*)
    |> printStack //

EmptyStack
|+ 10
|+ 10
|+ "string"
|> printStack
|> swap
|> printStack
|+ "11"
|> dup
|+ "string"
|> printStack
|> ignore

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
|!! (+)
|> printStack
|!! (+)
|> printStack
|> ignore

