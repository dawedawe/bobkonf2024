type ThingsBuilder() =

    member _.Yield(x) = [ x ]

    member _.Combine(currentThings, newThings) = currentThings @ newThings

    member _.Delay(f) = f ()

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
