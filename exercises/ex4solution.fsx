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
    member _.Sliced(w) = { w with Sliced = true }

    [<CustomOperation("unsliced")>]
    member _.Unsliced(w) = { w with Sliced = false }

    [<CustomOperation("spice")>]
    member _.Spice(w, s) = { w with Spicy = s }

    [<CustomOperation("add")>]
    member _.WithFries(w, a) = { w with Addons = a :: w.Addons }

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
