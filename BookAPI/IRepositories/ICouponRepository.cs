using BookAPI.Models;

namespace BookAPI.IRepositories;

public interface ICouponRepository : IRepository<Coupon>
{
    Task<Coupon?> GetByCodeAsync(string code);
}
