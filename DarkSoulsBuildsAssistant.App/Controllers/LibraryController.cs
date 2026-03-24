using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibraryController : ControllerBase
{
    private readonly ILibraryService _libraryService;

    public LibraryController(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books = await _libraryService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _libraryService.GetBookByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }
}