module Common

open System
    
    let generateName (prefix: string) =
        let randomNumber () = Random().Next(1000, 10000).ToString()
        prefix + randomNumber ()
