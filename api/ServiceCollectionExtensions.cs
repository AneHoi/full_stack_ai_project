using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using service.accountservice;

namespace api;

public static class ServiceCollectionExtensions
{
    /**
     * This will add a TokenService to the Interface IServiceCollection
     * "this" is a keyword for extending objects that we do not control. In this method we are extending the object
     * IServiceCollection, so we can use this extention method on the instance.
     * Like this with strings:
     * public static void PrintToConsole(this string message){Console.writeline(message);}
     * string demo = "Hello World";
     * demo.PrintToConsole();
     * The output: Hello World
     */
    public static void AddJwtService(this IServiceCollection services)
    {
        //IServiceCollection a container for registering and resolving services that can be used throughout an application
        // When using AddSingleton, a single instance of this IServiceCollection is created for the lifetime of the application
        services.AddSingleton<JwtOptions>(services =>
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            //The keyword comes from the appsettings.Development.json. It is a secret from there called JWT
            //The secret is used to sign and verify JWT's
            //A new secret can be produced in the terminal using: openssl rand -base64 64

            var options = configuration.GetRequiredSection("JWT").Get<JwtOptions>()!;
    
            // If address isn't set in the config then we are likely running in development mode.
            // We will use the address of the server as *issuer* for JWT.
            if (string.IsNullOrEmpty(options?.Address))
            {
                var server = services.GetRequiredService<IServer>();
                var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;
                options!.Address = addresses?.FirstOrDefault();
            }

            return options;
        });
        services.AddSingleton<JwtService>();
    }
}