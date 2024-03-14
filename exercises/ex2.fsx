// Let the type annotations guide you to the correct implementation.
// Hint: you can concatenate two lists like this: lst1 @ lst2

type ThingsBuilder() =

    member _.Yield(x: 'a) : 'a list = failwith "todo"

    member _.Combine(currentThings: 'a list, newThings: 'a list) : 'a list = failwith "todo"

    member _.Delay(f: unit -> 'a) : 'a = failwith "todo"

let things = ThingsBuilder()

// --------------------------------------------

let resultOfMultipleYields =
    things {
        yield 1
        yield 2
        yield 3
        yield 4
    }

printfn "multiple yields: %A" resultOfMultipleYields

let resultOfSingleYield = things { yield 1 }

printfn "single yield: %A" resultOfSingleYield
