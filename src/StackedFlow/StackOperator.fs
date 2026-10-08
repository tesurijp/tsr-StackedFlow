
namespace tsr.StackedFlow

module StackOperator =
    let (|+) stack value = Stack.push value stack
    let (|!) stack func = Stack.apply1 func stack
    let (|!!) stack func = Stack.apply2 func stack
    let (|!!!) stack func = Stack.apply3 func stack

