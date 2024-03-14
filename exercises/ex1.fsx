(*
Let the type annotations guide you to the correct implementation.
Hint: The definition of the Option type is:
type Option<'a> =
    | Some of 'a
    | None
*)

type MaybeBuilder() =

    // Option<'a> -> ('a -> Option<'b>) -> Option<'b>
    member _.Bind(x: Option<'a>, f: ('a -> Option<'b>)) : Option<'b> = failwith "todo"

    // 'a -> Option<'a>
    member _.Return(x: 'a) : Option<'a> = failwith "todo"

let maybe = MaybeBuilder()

// --------------------------------------------

let result =
    maybe {
        let! x = Some 1
        let! y = Some 2
        return x + y
    }


// Hint
let result' =
    let bind f m = maybe.Bind(m, f)
    let return' x = maybe.Return(x)

    Some 1 |> bind (fun x ->
    Some 2 |> bind (fun y ->
    return' (x + y)))
