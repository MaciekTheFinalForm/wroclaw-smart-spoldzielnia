namespace Api

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open DependencyResolution

type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    member this.ConfigureServices(services: IServiceCollection) =
        registerServices services
        services.AddMvc() |> ignore

    member this.Configure(app: IApplicationBuilder, _: IHostingEnvironment) =
        app
            .UseStatusCodePages()
            .UseMvc() |> ignore

    member val Configuration : IConfiguration = null with get, set