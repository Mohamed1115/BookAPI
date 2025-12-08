using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using BookAPI.Data;
using BookAPI.IRepositories;
using BookAPI.Models;


namespace BookAPI.Repositories;

public class AuthorRepository:Repository<Author>, IAuthorRepository
{
    public AuthorRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Author?> GetAuthorWithBooks(int id)
    {
        return await _context.Authors
            .Include(a => a.Books)
            .ThenInclude(ma => ma.Category)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}