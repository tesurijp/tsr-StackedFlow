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

    // for Debug
    let printStack stack =
        printfn "%O" stack
        stack
