namespace tsr.StackedFlow

module FuncApplyer =
    // direct apply
    let apply1 func stack = match stack with Stack(a1, tail) -> Stack(func a1, tail)
    let apply2 func stack = match stack with Stack(a2, Stack(a1, tail)) -> Stack(func a1 a2, tail)
    let apply3 func stack = match stack with Stack(a3, Stack(a2, Stack(a1, tail))) -> Stack(func a1 a2 a3, tail)
    let apply4 func stack = match stack with Stack(a4, Stack(a3, Stack(a2, Stack(a1, tail)))) -> Stack(func a1 a2 a3 a4, tail)
    let apply5 func stack = match stack with Stack(a5, Stack(a4, Stack(a3, Stack(a2, Stack(a1, tail))))) -> Stack(func a1 a2 a3 a4 a5, tail)

    // friendly name
    let unary = apply1
    let binary = apply2
    let ternary = apply3

    // use caller context
    let apply6 func stack =
        stack
        |> Caller.prepareArgs5  |> Caller.moreArgs1
        |> Caller.setFunc func 
        |> Caller.callArgs5 |> Caller.callArgs1
        |> Caller.finalize

    let apply7 func stack =
        stack
        |> Caller.prepareArgs5  |> Caller.moreArgs2
        |> Caller.setFunc func 
        |> Caller.callArgs5 |> Caller.callArgs2
        |> Caller.finalize

    let apply8 func stack =
        stack
        |> Caller.prepareArgs5  |> Caller.moreArgs2 |> Caller.moreArgs1
        |> Caller.setFunc func 
        |> Caller.callArgs5 |> Caller.callArgs2 |> Caller.callArgs1
        |> Caller.finalize

    let apply9 func stack =
        stack
        |> Caller.prepareArgs5  |> Caller.moreArgs2 |> Caller.moreArgs2
        |> Caller.setFunc func 
        |> Caller.callArgs5 |> Caller.callArgs2 |> Caller.callArgs2
        |> Caller.finalize

    let apply10 func stack =
        stack
        |> Caller.prepareArgs5  |> Caller.moreArgs5
        |> Caller.setFunc func 
        |> Caller.callArgs5 |> Caller.callArgs5
        |> Caller.finalize

