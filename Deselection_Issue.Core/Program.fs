namespace Program

open Elmish.WPF
open Serilog
open Serilog.Extensions.Logging


module Program =

    let main (window) =

        let logger =
            LoggerConfiguration()
                .MinimumLevel.Override("Elmish.WPF.Update", Events.LogEventLevel.Verbose)
                .MinimumLevel.Override("Elmish.WPF.Bindings", Events.LogEventLevel.Verbose)
                .MinimumLevel.Override("Elmish.WPF.Performance", Events.LogEventLevel.Verbose)
                .WriteTo.Console()
                .CreateLogger()

        WpfProgram.mkProgramT App.init App.update ViewModels.App_VM
        |> WpfProgram.withLogger (new SerilogLoggerFactory(logger))
        |> WpfProgram.startElmishLoop window
