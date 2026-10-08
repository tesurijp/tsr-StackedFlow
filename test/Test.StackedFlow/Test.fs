namespace Test.StakedFlow

open Microsoft.VisualStudio.TestTools.UnitTesting
open tsr.StackedFlow
open tsr.StackedFlow.Stack
open tsr.StackedFlow.StackOperator

[<TestClass>]
type StackOperationsTests() =

    let assertContents expected (stack: #IStack) =
        let expectedString = String.concat "::" expected

        Assert.AreEqual(expectedString, stack.ToString())

    [<TestMethod>]
    member _.``EmptyStack is empty`` () =
        Assert.IsNull((EmptyStack).ToString())

    [<TestMethod>]
    member _.``push adds values to the top in last-in first-out order`` () =
        let stack = EmptyStack |+ 1 |+ 2 |+ 3

        assertContents [ "3"; "2"; "1" ] stack

    [<TestMethod>]
    member _.``printStack returns the stack for continued pipeline processing`` () =
        let stack = EmptyStack |+ 1 |+ "text"

        assertContents [ "text"; "1" ] (printStack stack)

    [<TestMethod>]
    member _.``dup over drop swap rot pop and head operate on the top of a stack`` () =
        let stack = EmptyStack |+ 1 |+ 2 |+ 3

        assertContents [ "3"; "3"; "2"; "1" ] (dup stack)
        assertContents [ "2"; "3"; "2"; "1" ] (over stack)
        assertContents [ "2"; "1" ] (drop stack)
        assertContents [ "2"; "3"; "1" ] (swap stack)
        assertContents [ "1" ; "3"; "2" ] (rot stack)

        let value, remaining = pop stack
        Assert.AreEqual(3, value)
        assertContents [ "2"; "1" ] remaining
        Assert.AreEqual(3, head stack)

    [<TestMethod>]
    member _.``apply functions consume stack values from the top`` () =
        let stack = EmptyStack |+10 |+ 1 |+ 2 |+ 3 |+ 4

        assertContents [ "5"; "3"; "2"; "1" ; "10"] (apply1 ((+) 1) stack)
        assertContents [ "-1"; "2"; "1" ; "10"] (apply2 (-) stack)
        assertContents [ "9"; "1"; "10" ] (apply3 (fun a b c -> a + b + c) stack)

    [<TestMethod>]
    member _.``Caller applies one two and five arguments`` () =
        let stack = EmptyStack |+ 10 |+ 1 |+ 2 |+ 3 |+ 4

        let unary =
            stack
            |> Caller.prepareArgs1
            |> Caller.setFunc ((+) 1)
            |> Caller.callArgs1
            |> Caller.finalize

        let binary =
            stack
            |> Caller.prepareArgs2
            |> Caller.setFunc (-)
            |> Caller.callArgs2
            |> Caller.finalize

        let quinary =
            stack
            |> Caller.prepareArgs5
            |> Caller.setFunc (fun a b c d e -> a + b + c + d + e)
            |> Caller.callArgs5
            |> Caller.finalize

        assertContents [ "5"; "3"; "2"; "1"; "10" ] unary
        assertContents [ "-1"; "2"; "1"; "10" ] binary
        assertContents [ "20" ] quinary

    [<TestMethod>]
    member _.``Caller composes calls for twelve arguments and preserves the stack tail`` () =
        let result =
            EmptyStack
            |+ 99
            |+ 1
            |+ 2
            |+ 3
            |+ 4
            |+ 5
            |+ 6
            |+ 7
            |+ 8
            |+ 9
            |+ 10
            |+ 11
            |+ 12
            |> Caller.prepareArgs5
            |> Caller.moreArgs5
            |> Caller.moreArgs2
            |> Caller.setFunc (fun a b c d e f g h i j k l -> a + b + c + d + e + f + g + h + i + j + k + l)
            |> Caller.callArgs5
            |> Caller.callArgs5
            |> Caller.callArgs2
            |> Caller.finalize

        assertContents [ "78"; "99" ] result

    [<TestMethod>]
    member _.``pipeline operators are aliases for push and apply`` () =
        let transformed =
            EmptyStack
            |+ 1
            |+ 2
            |+ 3
            |! ((+) 1)
            |!! (-)

        let combined =
            EmptyStack
            |+ 1
            |+ 2
            |+ 3
            |!!! (fun a b c -> a + b + c)

        assertContents [ "-2"; "1" ] transformed
        assertContents [ "6" ] combined
