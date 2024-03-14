// Let the type annotations guide you to the correct implementation.

type ThingsBuilder() =

    member _.For(m: List<'a>, f: ('a -> List<'b>)) : List<'b> = m |> List.collect f

    member _.Return(x: 'a) = [ x ]

    member _.Zero() : List<'b> = []

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

(*
further exploration
- add yield support to the things CE
- make things a parameterized CE: things 23 { ... }
*)