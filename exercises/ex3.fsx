// Let the type annotations guide you to the correct implementation.

type ThingsBuilder() =

    member _.For(m: List<'a>, f: ('a -> List<'b>)) : List<'b> = failwith "todo"

    member _.Return(x: 'a) = failwith "todo"

    member _.Zero() : List<'b> = failwith "todo"

let things = ThingsBuilder()

// --------------------------------------------

let resultOfImplicitElse =
    things {
        if false then // play with the if condition
            return 23
    }

printfn "resultOfImplicitElse: %A" resultOfImplicitElse

let resultOfNestedFor =
    things {
        for i in [ 1..3 ] do
            for j in [ 10..12 ] do
                return i * j
    }

printfn "resultOfNestedFor: %A" resultOfNestedFor
