using BookAPI.IRepositories;
using BookAPI.Models;

namespace BookAPI.IRepositories;

public interface IAuthorRepository:IRepository<Author>
{
    Task<Author?> GetAuthorWithBooks(int id);
}