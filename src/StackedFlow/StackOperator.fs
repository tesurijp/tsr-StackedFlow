
namespace tsr.StackedFlow

module StackOperator =
    let (|+) stack value = Stack.push value stack
    let (|!) stack func = FuncApplyer.apply1 func stack
    let (|!!) stack func = FuncApplyer.apply2 func stack
    let (|!!!) stack func = FuncApplyer.apply3 func stack

