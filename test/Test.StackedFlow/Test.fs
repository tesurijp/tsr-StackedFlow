namespace Test.StakedFlow

open Microsoft.VisualStudio.TestTools.UnitTesting
open tsr.StackedFlow

[<TestClass>]
type StackOperationsTests () =

    let assertContents expected (stack: #IStack) =
        CollectionAssert.AreEqual(expected |> List.toArray, stack.ToStrings() |> List.toArray)

    [<TestMethod>]
    member _.``EmptyStack is empty`` () =
        assertContents [] EmptyStack

    [<TestMethod>]
    member _.``push adds values to the top in last-in first-out order`` () =
        let stack = EmptyStack |+ 1 |+ 2 |+ 3

        assertContents [ "3"; "2"; "1" ] stack

    [<TestMethod>]
    member _.``printStack returns the stack for continued pipeline processing`` () =
        let stack = EmptyStack |+ 1 |+ "text"

        assertContents [ "\"text\""; "1" ] (printStack stack)

    [<TestMethod>]
    member _.``dup drop swap pop and head operate on the top of a stack`` () =
        let stack = EmptyStack |+ 1 |+ 2 |+ 3

        assertContents [ "3"; "3"; "2"; "1" ] (dup stack)
        assertContents [ "2"; "1" ] (drop stack)
        assertContents [ "2"; "3"; "1" ] (swap stack)

        let value, remaining = pop stack
        Assert.AreEqual(3, value)
        assertContents [ "2"; "1" ] remaining
        Assert.AreEqual(3, head stack)

    [<TestMethod>]
    member _.``apply functions consume stack values from the top`` () =
        let stack = EmptyStack |+ 1 |+ 2 |+ 3 |+ 4

        assertContents [ "5"; "3"; "2"; "1" ] (apply1 ((+) 1) stack)
        assertContents [ "1"; "2"; "1" ] (apply2 (-) stack)
        assertContents [ "9"; "1" ] (apply3 (fun a b c -> a + b + c) stack)
        assertContents [ "10" ] (apply4 (fun a b c d -> a + b + c + d) stack)

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

        let fourValues =
            EmptyStack
            |+ 1
            |+ 2
            |+ 3
            |+ 4
            |!!!! (fun a b c d -> a + b + c + d)

        assertContents [ "2"; "1" ] transformed
        assertContents [ "6" ] combined
        assertContents [ "10" ] fourValues
