using YourDarkSoulsAssistant.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace YourDarkSoulsAssistant.App.Controllers.API;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class TestDbController(BuildsAssistantDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> CheckDatabase()
    {
        try
        {
            // 1. Перевірка фізичного з'єднання
            if (!await context.Database.CanConnectAsync())
                return StatusCode(500, "❌ Помилка: Неможливо фізично під'єднатися до сервера БД.");

            // 2. Спроба витягнути дані
            // Важливо: Ми використовуємо Select, щоб уникнути циклічних посилань 
            // (Circular Reference), бо EF почне тягнути User -> Roles -> Users...
            var users = await context.Users
                .AsNoTracking()
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    // Ліземо в таблицю зв'язків через контекст
                    RoleCount = context.UserRoles.Count(ur => ur.UserId == u.Id),

                    // А якщо хочете витягнути назви ролей (трохи складніше):
                    Roles = context.UserRoles
                        .Where(ur => ur.UserId == u.Id)
                        .Join(context.Roles,
                            ur => ur.RoleId,
                            r => r.Id,
                            (ur, r) => r.Name)
                        .ToList()
                })
                .Take(5)
                .ToListAsync();

            return Ok(new
            {
                Status = "✅ Успіх! База відповідає.",
                DbName = context.Database.GetDbConnection().Database,
                UsersFound = users.Count,
                Data = users
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Status = "🔥 Вибух!",
                Error = ex.Message,
                Details = ex.InnerException?.Message
            });
        }
    }
}