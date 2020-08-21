type ResultListBuilder () =
    member this.Bind(x, f) = 
        match x with
        | Ok c -> f c
        | Error c -> Error c

    member this.Delay f = f ()

    member this.Return x : Result<'a, 'c> = Ok x

    member this.ReturnFrom x =
        match x with
        | Ok c -> Ok c
        | Error c -> Error c

    member this.Zero () = Ok ()

    member this.For(cs : Result<'a, 'b> seq, f : 'a -> Result<'a, 'b>) : Result<'a list, 'b list> =
        let candidates = cs |> Seq.map (function | Ok c -> f c | Error c -> Error c) |> Seq.toList
        match candidates |> List.filter (function | Error _ -> true | Ok _ -> false) with
        | [] -> candidates |> List.map (function Ok c -> c | Error _ -> failwith "impossible") |> Ok
        | errors -> errors |> List.map (function Error c -> c | Ok _ -> failwith "impossible") |> Error

    member this.Yield x = Ok x
    member this.YieldFrom x = 
        match x with
        | Ok c -> Ok c
        | Error c ->  Error c

let resultList = ResultListBuilder ()
