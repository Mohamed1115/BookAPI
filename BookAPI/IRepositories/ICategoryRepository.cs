using BookAPI.Models;

namespace BookAPI.IRepositories;

public interface ICategoryRepository:IRepository<Category>
{
    Task<Category?> GetCategoryWithMovies(int id);
}