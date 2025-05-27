using GrpcRemoteProcessInvoker.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace GrpcService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Additional configuration is required to successfully run gRPC on macOS.
            // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
            builder.WebHost.ConfigureKestrel(options =>
            {
                // Setup a HTTP/2 endpoint without TLS for development
                options.ListenLocalhost(5099, o => o.Protocols = HttpProtocols.Http2);
            });

            // Add services to the container.
            builder.Services.AddGrpc();
            builder.Services.AddSingleton<CommandExecutor>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<RemoteProcessService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

            app.Run();
        }
    }
}