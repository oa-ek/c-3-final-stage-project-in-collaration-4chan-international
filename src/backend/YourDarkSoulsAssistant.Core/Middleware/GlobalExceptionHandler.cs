using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using YourDarkSoulsAssistant.Core.DTOs;

namespace YourDarkSoulsAssistant.Core.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "--> [GlobalError]: Перехоплено невідловлену помилку: {Message}", exception.Message);
        
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";
        
        var response = HTTPResult<object>.Failure(
            "Сталася внутрішня помилка сервера. Ми вже працюємо над цим.");

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        
        return true; 
    }
}
