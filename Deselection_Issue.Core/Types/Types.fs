module Types

open System

module X =

    type Model = { 
        Id: Guid 
        Text: string             
    }
    
    let create () = { 
        Id = Guid.NewGuid() 
        Text = "Text_" 
    }
    
    type Msg = 
        | SetText of string
    
    let init () = create ()
    
    let update msg m = 
        match msg with 
        | SetText text -> { m with Text = text }
    