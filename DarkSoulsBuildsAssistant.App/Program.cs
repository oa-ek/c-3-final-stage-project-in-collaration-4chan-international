using DarkSoulsBuildsAssistant.App.Extensions;
using Scalar.AspNetCore;
using DarkSoulsBuildsAssistant.App.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.ValidateEnvironmentVariables();

builder.Services.AddDatabaseAndIdentity(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddWebConfiguration(builder.Configuration);
builder.Services.AddOpenApiDocumentation();

var app = builder.Build();

app.UseStaticFiles();
app.UseForwardedHeaders();

app.UseAuthentication();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<JWTBlacklistMiddleware>();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
    
    app.MapOpenApi();
    
    var devToken = builder.Configuration["Jwt:DevToken"];

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Dark Souls Assistant API")
            .WithTheme(ScalarTheme.Mars)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithOpenApiRoutePattern("/openapi/v1.json")
            
            .WithPreferredScheme("Bearer") 
            
            .WithHttpBearerAuthentication(bearer => 
            {
                bearer.Token = devToken ?? "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9rjlkg;halfbhgethhhhhnhhhghkjfafgdhlkjrhyiuthqlgrkjrelkjbaelrthgqiuy43";
            });
    });
    
    app.UseCors("AllowAll");
}
else
{
    app.UseCors("AllowReact");
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapRazorPages();

app.Run();
