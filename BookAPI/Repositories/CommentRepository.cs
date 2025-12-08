using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using BookAPI.Data;
using BookAPI.IRepositories;
using BookAPI.Models;


namespace BookAPI.Repositories;

public class CommentRepository:Repository<Comment>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Comment>> GetCommentDetails(int id)
    {
        return await _context.Comments
            .Where(c => c.BookId == id)
            .Include(c => c.User)
            .ToListAsync();
    }
    
    public async Task<float> GetAverageRatingAsync(int bookId)
    {
        return await _context.Comments
            .Where(c => c.BookId == bookId)
            .AverageAsync(c => (float)c.Rating);
    }
    
}