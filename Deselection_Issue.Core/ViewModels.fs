namespace ViewModels

open Elmish.WPF
open Types


//[<AllowNullLiteral>]
//type Child_VM (args) =
//        inherit ViewModelBase<Child.Model, Child.Msg>(args)

//        //• helpers

//        //• members


[<AllowNullLiteral>]
type App_VM(args) =
    inherit ViewModelBase<App.Model, App.Msg>(args)

    //• helpers


    //• members
    new() = App_VM(App.init () |> fst |> ViewModelArgs.simple)


//        //• SubModels
//        member _.Child_VM =
//            base.Get
//                ()
//                (Binding.SubModelSeqKeyedT.id Child_VM _.Id
//                    >> Binding.mapModel App.ModelM.Child.get
//                    >> Binding.mapMsg (fun msg -> App.Msg.Child_Msg msg) // ou snd?
//                )
