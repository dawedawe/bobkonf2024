# Computation Expressions in F#

### by Ronald Schlenker and David Schaefer

---

# Agenda

- Introduction of speakers
- Why? What's the motivation?
- Theoretic Basics of CEs
- Examples of real world CEs
- Tutorial

---

## David Schaefer (dawe)
- located in Cologne
- freelancing software engineer with a big ❤️‍🔥 for functional programming
- working on the F# ecosystem, focus on compiler and dev tooling
- member of [AmplifyingFSharp](https://amplifyingfsharp.io/)
- https://github.com/dawedawe
- https://fosstodon.org/@dawe

---

Let's see Ronald's recording

---

What is a Computation Expression(CE)?

All of the following:
```fsharp
expr { for ... }
expr { let ... }
expr { let! ... }
expr { and! ... }
expr { use ... }
expr { do! ... }
expr { match! ... }
expr { while ... }
expr { while! ... }
expr { yield ... }
expr { yield! ... }
expr { try ... }
expr { return ... }
expr { return! ... }
// + custom operations
```

---

## Examples of real world CEs

---

`async` Example

```fsharp
async {
    let! r1 = someAsyncReturningFunc1 23
    let! r2 = someAsyncReturningFunc2 42
    let! r3 = someAsyncReturningFunc3 99
    return r1 + r2 + r3
}
```

---

[Validus](https://github.com/pimbrouwers/Validus)  
A `let!` with different semantics

```fsharp	
validate {
    let! first = nameValidator "First name" dto.FirstName
    and! last = nameValidator "Last name" dto.LastName
    and! email = emailValidator "Email address" dto.Email
    and! age = ageValidator "Age" dto.Age
    and! startDate = dateValidator "Start Date" dto.StartDate
    // Construct Person if all validators return Success
    return {
        Name = { First = first; Last = last }
        Email = email
        Age = age
        StartDate = startDate }
}
```

---

The flexibility continues: List Comprehensions

```fsharp
myList {
    yield 0
    1
    2
    for x in numbers do
        x + 100
}
```

---

[Vide](https://vide-dev.io/)
```fsharp
// compose state-aware DSP functions as if they were pure:
dsp {
    let! sig = Osc.noise
    let! lfo = Osc.sin 7.0
    let modulatedSig = sig * lfo
    let! filteredSig =
        { cutoff = 3000.0; resonance = 0.5 }
        |> Filter.lowPass modulatedSig
    return filteredSig
}
|> Dsp.loopUntil cancellation
```

---

[FsHttp](https://fsprojects.github.io/FsHttp/)  
A hackable HTTP client (using custom operations)

```fsharp
http {
    POST "https://reqres.in/api/users"
    CacheControl "no-cache"
    body
    jsonSerialize
        {|
            name = "morpheus"
            job = "leader"
        |}
}
|> Request.send
```

---

[Farmer](https://compositionalit.github.io/farmer/)  
Making repeatable Azure deployments easy!

```fsharp

// Create a storage account with a container
let myStorageAccount = storageAccount {
    name "myTestStorage"
    add_public_container "myContainer"
}

// Create a web app with application insights that's connected to the storage account.
let myWebApp = webApp {
    name "myTestWebApp"
    setting "storageKey" myStorageAccount.Key
}

// Create an ARM template
let deployment = arm {
    location Location.NorthEurope
    add_resources [
        myStorageAccount
        myWebApp
    ]
}
```

---

Query expressions in F# (LINQ), implemented as a CE
```fsharp
query {
    for p in db.Products do
    where (p.Price > 100.0M)
    groupBy p.Category into g
    select (g.Key, g.Count())
}
```

---

[F# Compiler Tests](https://github.com/dotnet/fsharp/blob/main/tests/FSharp.Compiler.ComponentTests/FSharpChecker/TransparentCompiler.fs)  
again, custom operations make DX better

```fsharp
ProjectWorkflowBuilder(project) {
    tryGetRecentCheckResults "First" expectSome
    updateFile "First" updatePublicSurface
    tryGetRecentCheckResults "First" expectNone
    checkFile "First" expectOk
    tryGetRecentCheckResults "First" expectSome
} |> ignore
```

---

## How does that work?

- Compiler translates (desugars) expressions inside a CE (inside `{}`)
- For a custom CE, you define the target logic of this rewriting process
- Your costum code goes into a so called `builder type`
- A `builder type` is an F# class type defining [members adhering to a spec](https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/computation-expressions#creating-a-new-type-of-computation-expression) that the compiler expects

---

Example of a builder type:

```fsharp
type MyFirstCEBuilder() =
    
    member _.Bind(x: M<'a>, f: ('a -> M<'b>)) =
        let unwrappedX = unwrap x
        f unwrappedX

    member _.Return(x: 'a) : M<'a> =
        wrap x

let myfirstce = MyFirstCEBuilder()
```

---

```fsharp
let result =
    myfirstce {
        let! x = M 1
        let! y = M 2
        return x + y
    }
```

---

## What's the deal witht the `M`?

- As you might have guessed already, CEs give you Monads in F# (and much more)
- Every CE makes use of a generic wrapper type, e.g. `M<'t>`, aka the underlying type of the CE

---

## Desugaring

The compiler desugars CEs in a recursive way.

Example:  
A CE like  
```fsharp
{{ let! pattern = expr in cexpr }}
```
is desugard into  
```fsharp
builder.Bind(expr, (fun pattern -> {{ cexpr }}))
```

Demo

---

## Custom operations

You can define custom operations for your CE:

```fsharp
type TortillaBuilder() =

    [<CustomOperation "add">]
    member _.AddFixing (state:Tortilla, fixing) =
        match state.Fixings with
        | None -> { state with Fixings = Some (Fixings.add Fixings.Create fixing) }
        | Some fixings -> { state with Fixings = Some (Fixings.add fixings fixing) }
```

Usage:
```fsharp
let t = tortilla {
    condition Soft
    add Meat
    add Rice
    notfried
}
```

---

# Tutorial

- Clone https://www.github.com/dawedawe/bobkonf2024

- Switch off copilot for a better learning experience

---

Thanks for attending.  
Enjoy the rest of bobkonf 2024!

---

## References

[CE Language Reference](https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/computation-expressions)

[The F# Computation Expression Zoo](https://tomasp.net/academic/papers/computation-zoo/computation-zoo.pdf)

[Scott Wlaschin: For Fun and Profit](https://fsharpforfunandprofit.com/posts/computation-expressions-intro/)

[Vide](https://vide-dev.io/)

[Validus](https://github.com/pimbrouwers/Validus)

[Farmer](https://compositionalit.github.io/farmer/)