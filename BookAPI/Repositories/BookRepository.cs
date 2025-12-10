using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using BookAPI.Data;
using BookAPI.IRepositories;
using BookAPI.Models;


namespace BookAPI.Repositories;

public class BookRepository:Repository<Book>, IBookRepository
{
    public BookRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Book?> GetBookDetails(int id)
    {
        return await _context.Books
            .Include(a => a.Author)
            .Include(a => a.Category)
            .Include(a => a.Comments)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    public async Task<List<Book>> GetAllBook()
    {
        return await _context.Books
            .Include(a => a.Author)
            .Include(a => a.Category)
            .ToListAsync();
    }
    
    
    
    
}