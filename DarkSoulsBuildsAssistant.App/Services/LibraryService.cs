using Data.Context; // Ваш DbContext
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Shared.DTO.Library_item;
using Shared.Dtos;

namespace Services.Contracts;

public class LibraryService : ILibraryService
{
    private readonly LibrarySystemDbContext _context;

    public LibraryService(LibrarySystemDbContext context)
    {
        _context = context;
    }

    public async Task<List<LibraryItemDto>> GetAllBooksAsync()
    {
        // Мапимо модель БД в DTO
        return await _context.LibraryItems
            .Select(b => new LibraryItemDto
            {
                Id = b.ItemId,
                Title = b.Title,
                Author = b.Authors.ToString(),
                // Description = b.Description, // Якщо є в БД
                // Додайте інші поля
            })
            .ToListAsync();
    }

    public async Task<LibraryItemDto?> GetBookByIdAsync(int id)
    {
        var b = await _context.LibraryItems.FindAsync(id);
        if (b == null) return null;

        return new LibraryItemDto
        {
            Id = b.ItemId,
            Title = b.Title,
            Author = b.Authors.ToString(),
            // Description = b.Description, // Якщо є в БД
            // Додайте інші поля
        };
    }
}