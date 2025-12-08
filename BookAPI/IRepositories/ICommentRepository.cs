using BookAPI.IRepositories;
using BookAPI.Models;

namespace BookAPI.IRepositories;

public interface ICommentRepository:IRepository<Comment>
{
    Task<List<Comment>> GetCommentDetails(int id);
    Task<float> GetAverageRatingAsync(int bookId);
}