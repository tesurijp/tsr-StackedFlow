namespace tsr.StackedFlow

module Caller =
    type PreCallContext<'argStack, 'remainStack> = private PreCallContext of 'argStack * 'remainStack
    type CallContext<'argStack, 'remainStack> = private CallContext of 'argStack * 'remainStack

    let private addArg1 context =
        match context with PreCallContext(dest, Stack(a, tail)) ->
            PreCallContext(Stack(a, dest), tail)
    let private addArg2 context =
        match context with PreCallContext(dest, Stack(a2, Stack(a1, tail))) ->
            PreCallContext(Stack(a1, Stack(a2, dest)), tail)
    let private addArg5 context =
        match context with PreCallContext(dest, Stack(a5, Stack(a4, Stack(a3, Stack(a2, Stack(a1, tail)))))) ->
            PreCallContext(Stack(a1, Stack(a2, Stack(a3, Stack(a4, Stack(a5, dest))))), tail)

    // Initialize precall context
    let prepareArgs1 from = PreCallContext(EmptyStack, from) |> addArg1
    let prepareArgs2 from = PreCallContext(EmptyStack, from) |> addArg2
    let prepareArgs5 from = PreCallContext(EmptyStack, from) |> addArg5

    // Update arguments
    let moreArgs1 = addArg1 
    let moreArgs2 = addArg2
    let moreArgs5 = addArg5

    // Switch to call context
    let setFunc func context =
        match context with PreCallContext(dest, stack) ->
            CallContext(Stack(func, dest), stack)

    // Invoke
    let callArgs1 context = 
        match context with CallContext(Stack(func, Stack(a1, tail)), stack ) ->
            CallContext(Stack(func a1 , tail), stack)
    let callArgs2 context = 
        match context with CallContext(Stack(func, Stack(a1, Stack(a2, tail))), stack) -> 
            CallContext(Stack(func a1 a2, tail), stack)
    let callArgs5 context = 
        match context with CallContext(Stack(func, Stack(a1, Stack(a2, Stack(a3, Stack(a4, Stack(a5, tail)))))), stack)-> 
            CallContext(Stack(func a1 a2 a3 a4 a5, tail), stack)

    // Go back
    let finalize context = 
        match context with CallContext(Stack(result, EmptyStack),  stack) ->
            Stack(result, stack)
