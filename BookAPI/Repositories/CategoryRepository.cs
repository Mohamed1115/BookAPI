using BookAPI.Data;
using BookAPI.IRepositories;
using BookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories;

public class CategoryRepository:Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Category?> GetCategoryWithMovies(int id)
    {
        return await _context.Categories
            .Include(a => a.Books)
            .ThenInclude(m => m.Author)
            .FirstOrDefaultAsync(a => a.Id == id);
            
    }
}