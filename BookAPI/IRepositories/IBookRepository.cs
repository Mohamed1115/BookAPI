using BookAPI.IRepositories;
using BookAPI.Models;

namespace BookAPI.IRepositories;

public interface IBookRepository:IRepository<Book>
{
    Task<Book?> GetBookDetails(int id);
    Task<List<Book>> GetAllBook();
}