// Let the type annotations guide you to the correct implementation.

type Spicyness =
    | Low
    | Medium
    | High
    | Hell

type Addon =
    | Fries
    | Bread

type Wurst =
    { Sliced: bool
      Spicy: Spicyness
      Addons: Addon list }

    static member Default =
        { Sliced = true
          Spicy = Medium
          Addons = [] }

type CurrywurstBuilder() =

    member _.Yield _ = Wurst.Default

    [<CustomOperation("sliced")>]
    member _.Sliced(w: Wurst) = { w with Sliced = true }

    [<CustomOperation("unsliced")>]
    member _.Unsliced(w: Wurst) = failwith "todo"

    [<CustomOperation("spice")>]
    member _.Spice(w: Wurst, s: Spicyness) : Wurst = failwith "todo"

    [<CustomOperation("add")>]
    member _.WithFries(w: Wurst, a: Addon) : Wurst = failwith "todo"

let currywurst = CurrywurstBuilder()

// --------------------------------------------

let slicedHellyAllAddons =
    currywurst {
        sliced
        spice Hell
        add Fries
        add Bread
    }

printfn "bon appetit: %A" slicedHellyAllAddons
