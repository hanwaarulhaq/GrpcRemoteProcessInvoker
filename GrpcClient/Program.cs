using Grpc.Net.Client;
using GrpcRemoteProcessInvoker;
using System.CommandLine;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var portOption = new Option<int>(
            name: "--port",
            description: "The port the gRPC server is listening on",
            getDefaultValue: () => 5099);
        var hostOption = new Option<string>(
            name: "--host",
            description: "The host the gRPC server is running on",
            getDefaultValue: () => "localhost");

        var rootCommand = new RootCommand("gRPC Remote Process Invoker Client");
        rootCommand.AddOption(portOption);
        rootCommand.AddOption(hostOption);

        rootCommand.SetHandler(async (host, port) =>
        {
            using var channel = GrpcChannel.ForAddress($"http://{host}:{port}");
            var client = new RemoteProcessInvoker.RemoteProcessInvokerClient(channel);

            while (true)
            {
                Console.Write("Enter command to execute (or 'exit' to quit): ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var command = parts[0];
                var arguments = parts.Length > 1 ? parts[1..] : Array.Empty<string>();

                var request = new CommandRequest
                {
                    Command = command,
                };
                request.Arguments.AddRange(arguments);

                try
                {
                    var reply = await client.ExecuteCommandAsync(request);
                    Console.WriteLine(reply.Success ? "Command executed successfully" : "Command failed");
                    Console.WriteLine("Output:");
                    Console.WriteLine(reply.Output);

                    if (!string.IsNullOrEmpty(reply.Error))
                    {
                        Console.WriteLine("Error:");
                        Console.WriteLine(reply.Error);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing command: {ex.Message}");
                }

                Console.WriteLine();
            }
        }, hostOption, portOption);

        await rootCommand.InvokeAsync(args);
    }
}