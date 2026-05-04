using Microsoft.AspNetCore.Mvc;

namespace YourDarkSoulsAssistant.UsersService.Controllers.Api;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("")]
    public void Root()
    {
        Results.Redirect("/scalar/v1");
    }

    [HttpGet("ping")]
    public string Ping()
    {
        return "User Service is running!";
    }
}
