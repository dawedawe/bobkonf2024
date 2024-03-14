// With the implementation in place, we don't need the type annotatins anymore.

type MaybeBuilder() =
    // Option<'a> -> ('a -> Option<'b>) -> Option<'b>
    member _.Bind(x, f) =
        match x with
        | Some x -> f x
        | None -> None

    // 'a -> Option<'a>
    member _.Return(x) = Some x

let maybe = MaybeBuilder()

// --------------------------------------------

let result =
    maybe {
        let! x = Some 1
        let! y = Some 2
        return x + y
    }


(*
further exploration
- add Delay and Run members
*)