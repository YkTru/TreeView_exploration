module App

open System
open Elmish

open Types


type Model =
    { Id: Guid
      Text: string
    //• SubModels

     }

let create () =
    { Id = Guid.NewGuid()
      Text = "Text_" }

module ModelM =
    module Child1 =
        let get = fun m -> m.Text


type Msg = SetText of string
//• SubModels


let init () = create (), Cmd.none
//• SubModels


module UpdateHelper =
    ()


let update msg m : Model * Cmd<Msg> =
    match msg with
    | SetText text -> { m with Text = text }, Cmd.none
//• SubModels
