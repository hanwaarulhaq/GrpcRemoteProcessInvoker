using Grpc.Core;

namespace GrpcRemoteProcessInvoker.Services;

public class RemoteProcessService : RemoteProcessInvoker.RemoteProcessInvokerBase
{
    private readonly ILogger<RemoteProcessService> _logger;
    private readonly CommandExecutor _commandExecutor;

    public RemoteProcessService(ILogger<RemoteProcessService> logger, CommandExecutor commandExecutor)
    {
        _logger = logger;
        _commandExecutor = commandExecutor;
    }

    public override async Task<CommandResponse> ExecuteCommand(CommandRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Executing command: {Command} with args: {Arguments}",
            request.Command, string.Join(" ", request.Arguments));

        try
        {
            var result = await _commandExecutor.ExecuteAsync(request.Command, request.Arguments);
            return new CommandResponse
            {
                Success = result.Success,
                Output = result.Output,
                Error = result.Error ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing command");
            return new CommandResponse
            {
                Success = false,
                Error = ex.Message
            };
        }
    }
}