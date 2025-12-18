using System;
using RepositoryContracts;
using Grpc.Core;

namespace RESTAPI;

public class GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger) : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException nEx)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(nEx.Message);
            _logger.LogWarning(nEx, "Resource not found.");
        }
        catch (RpcException rpcEx) when (rpcEx.StatusCode == StatusCode.NotFound)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync("Resource not found");
            _logger.LogWarning(rpcEx, "gRPC resource not found.");
        }
        catch (UnauthorizedAccessException uaEx)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(uaEx.Message);
            _logger.LogWarning(uaEx, "Unauthorized access.");
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(ex.Message);
            _logger.LogError(ex, "An unexpected error occurred.");
        }
    }
}
