using BookAPI.Models;

namespace BookAPI.IRepositories;

public interface ICartRepository:IRepository<Cart>
{
    Task<List<Cart>> GetByIdUserAsync(string userId);
    Task<Cart?> GetByBookAndUserAsync(int movieId, string userId);
    Task DeleteByUserAsync(string userId);

}