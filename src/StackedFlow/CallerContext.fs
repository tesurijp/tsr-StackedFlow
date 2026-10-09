namespace tsr.StackedFlow

module Caller =
    type Init = private | Init 
    type Step<'inner> = private Step of  'inner
    type PreCallContext<'args, 'argStack, 'remainStack> = private PreCallContext of 'args * 'argStack * 'remainStack
    type CallContext<'args, 'argStack, 'remainStack> = private CallContext of 'args * 'argStack * 'remainStack

    let private addArg1 context =
        match context with PreCallContext(args, dest, Stack(a, tail)) ->
            PreCallContext( Step(args), Stack(a, dest), tail)
    let private addArg2 context =
        match context with PreCallContext(args, dest, Stack(a2, Stack(a1, tail))) ->
            PreCallContext( Step(Step(args)), Stack(a1, Stack(a2, dest)), tail)
    let private addArg5 context =
        match context with PreCallContext(args, dest, Stack(a5, Stack(a4, Stack(a3, Stack(a2, Stack(a1, tail)))))) ->
            PreCallContext( Step(Step(Step(Step(Step(args))))), Stack(a1, Stack(a2, Stack(a3, Stack(a4, Stack(a5, dest))))), tail)

    // Initialize precall context
    let prepareArgs1 from = PreCallContext(Init, EmptyStack, from) |> addArg1
    let prepareArgs2 from = PreCallContext(Init, EmptyStack, from) |> addArg2
    let prepareArgs5 from = PreCallContext(Init, EmptyStack, from) |> addArg5

    // Update arguments
    let moreArgs1 = addArg1 
    let moreArgs2 = addArg2
    let moreArgs5 = addArg5

    // Switch to call context
    let setFunc func context =
        match context with PreCallContext(args, dest, stack) ->
            CallContext(args, Stack(func, dest), stack)

    // Invoke
    let callArgs1 context = 
        match context with CallContext( Step(result), Stack(func, Stack(a1, tail)), stack ) ->
            CallContext(result, Stack(func a1 , tail), stack)
    let callArgs2 context = 
        match context with CallContext( Step(Step(result)), Stack(func, Stack(a1, Stack(a2, tail))), stack) -> 
            CallContext(result, Stack(func a1 a2, tail), stack)
    let callArgs5 context = 
        match context with CallContext( Step(Step(Step(Step(Step(result))))), Stack(func, Stack(a1, Stack(a2, Stack(a3, Stack(a4, Stack(a5, tail)))))), stack)-> 
            CallContext(result, Stack(func a1 a2 a3 a4 a5, tail), stack)

    // Go back
    let finalize context = 
        match context with CallContext(args : Init, Stack(result, _),  stack) ->
            Stack(result, stack)
