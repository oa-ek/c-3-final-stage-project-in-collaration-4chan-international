using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Identity;

using System.Net;

namespace YourDarkSoulsAssistant.App.Middleware;

public class JWTBlacklistMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ITokenBlacklistService blacklistService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            if (await blacklistService.IsTokenBlacklistedAsync(token))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Token is revoked.");
                return;
            }

        await next(context);
    }
}
