# ResultBuilder

A simple computational expression builder to work with `Result<'a, 'b>`.

## Working with values

You can use it to construct a `Result` from simple values (though that might not make much sense ;)

```fs
let value : Result<int, unit> = result {
    return 0
} // Ok 0
```

It can help, when working with the inner values of `Result` – for instance:

``` fs
let value : Result<int, unit> = result { 
    let! a = Ok 1
    let! b = Ok 2

    return a + b
} // Ok 3
```

The benefit is the handling of `Error` values:

```fs
let value : Result<int, string> = result { 
    let! a = Ok 1
    let! b = Error "oh no :("

    return a + b
} // Error "oh no :("

```

## Working with collections

While the above examples can also be done with pattern matching, it gets trickier when working with collections.
The semantic of the `ResultBuilder` is, when processing a list of `Result` values, either there are only `Ok` values, and you get an `Ok` of a list of all values. 
If there is at least one `Error` value, you get an `Error` of a list of all encountered errors.
For example:

```fs
let cs : Result<int, string> = [
    Ok 1
    Ok 2
    Ok 3
    Error "4"
    Ok 5
    Error "6"
]

let a : Result<int list, string list> = result {
    for (c : int) in cs do
        yield c + 1
} // Error ["4"; "6"]

let a' = result {
    for c in cs |> List.take 3 do
        yield c + 1
} // Ok [2; 3; 4]
```

`for` constructs do not have to iterate over a value of type `Result`:

```fs
let b = result {
    for (i, c) in cs |> List.indexed do
        let! c = c // c is now Result<int, string> and must be unwrapped
        yield c + i
}
```
