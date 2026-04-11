using System.Security.Claims;

namespace YourDarkSoulsAssistant.App.Middleware;

public class ApiKeyMiddleware(RequestDelegate next)
{
    private const string ApiKeyHeaderName = "X-Api-Key";

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        var validApiKey = configuration["App:ApiKey"];
        
        if (context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedApiKey))
        {
            if (!string.IsNullOrEmpty(validApiKey) && validApiKey.Equals(providedApiKey))
            {
                var claims = new[] { new Claim(ClaimTypes.Role, "SystemApp") };
                var identity = new ClaimsIdentity(claims, "ApiKeyAuth");
                
                context.User.AddIdentity(identity);
            }
        }
        
        await next(context);
    }
}