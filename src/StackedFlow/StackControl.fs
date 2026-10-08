namespace tsr.StackedFlow

module Stack =
    // Manupilate stack
    let dup stack = match stack with Stack(head, _) -> Stack(head, stack)
    let over stack = match stack with Stack(_, Stack(h2, tail)) -> Stack(h2, stack)
    let drop stack = match stack with Stack(_, tail) -> tail
    let swap stack = match stack with Stack(h1, Stack(h2, tail)) -> Stack(h2, Stack(h1, tail))
    let rot stack = match stack with Stack(h1, Stack(h2, Stack(h3, tail))) -> Stack(h3, Stack(h1, Stack(h2, tail)))
    let pop stack = match stack with Stack(head, tail) -> head, tail
    let head stack = match stack with Stack(head, _) -> head
    let push value stack = Stack(value, stack)

    // Apply stack 
    let apply1 func stack = match stack with Stack(a1, tail) -> Stack(func a1, tail)
    let apply2 func stack = match stack with Stack(a2, Stack(a1, tail)) -> Stack(func a1 a2, tail)
    let apply3 func stack = match stack with Stack(a3, Stack(a2, Stack(a1, tail))) -> Stack(func a1 a2 a3, tail)

    let unary = apply1
    let binary = apply2
    let ternary = apply3

    // for Debug
    let printStack stack =
        printfn "%O" stack
        stack
