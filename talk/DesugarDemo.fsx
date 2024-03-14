
open FSharp.Quotations
open FSharp.Quotations.Patterns

let rec printExpr tabLevel (e: Expr) =
    let tabs = String.replicate tabLevel "\t"
    let noLevel = printExpr 0
    let sameLevel = printExpr tabLevel
    let indented = printExpr (tabLevel + 1)
    match e with
    | Call (Some(ValueWithName(blah, ty, name)), method, parameters) ->
        let parameters = parameters |> List.map indented |> String.concat (sprintf ",\n")
        sprintf "%s%s.%s(\n%s\n%s)" tabs name method.Name parameters tabs
    | Call (None, method, parameters) ->
        let parameters = parameters |> List.map indented |> String.concat (sprintf ",\n")
        sprintf "%s%s(\n%s\n%s)" tabs method.Name parameters tabs
    | NewUnionCase(case, []) -> case.Name
    | NewUnionCase(case, [parameter]) ->
        sprintf "%s%s %s" tabs case.Name (noLevel parameter)
    | NewUnionCase(case, parameters) ->
        let parameters = parameters |> List.map noLevel |> String.concat ", "
        sprintf "%s%s(%s)" tabs case.Name parameters
    | Lambda(var, expr) ->
        sprintf "%sfun %s ->\n%s" tabs var.Name (indented expr)
    | Let (var, e1, e2) ->
        sprintf "%slet %s = %s\n%s" tabs var.Name (noLevel e1) (sameLevel e2)
    | TupleGet (expr, index) ->
        sprintf "%s%s.Item%d" tabs (noLevel expr) index
    | Value (obj, ty) -> sprintf "%s%A" tabs obj
    | NewTuple(exprs) ->
        sprintf "%s(%s)" tabs (exprs |> List.map noLevel |> String.concat ", ")
    | IfThenElse(condition, trueBr, falseBr) ->
        sprintf "%sif %s\n%sthen\n%s\n%selse\n%s"
            tabs
            (noLevel condition)
            tabs
            (indented trueBr)
            tabs
            (indented falseBr)
    | e ->
        sprintf "%s%A" tabs e

type Builder() =

    member this.Bind(m, f) =
        match m with
        | Some x -> f x
        | None -> m

    member this.Return(x) = Some x

    member this.Quote(exp : Expr<_>) : Expr<_> =
        exp

let builder = new Builder()

let sugared =
    builder {
        let x = 11
        let! y = Some 22
        let! z = Some 33
        return x + y + z
    }


printExpr 1 sugared