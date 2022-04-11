using ECommerceRestApi.DAL;
using ECommerceRestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceRestApi.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart> GetByUserUsernameAsync(string username)
    {
        AppUser? user = await _context.AppUsers
            .Include(user => user.Cart)
            .ThenInclude(cart => cart.Items)
            .ThenInclude(item => item.Product)
            .FirstOrDefaultAsync(user => user.Username == username);

        if (user is null)
        {
            throw new ArgumentException("User with given username does not exist.");
        }

        return user.Cart;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}

public interface ICartRepository
{
    public Task<Cart> GetByUserUsernameAsync(string username);
    public Task SaveAsync();
}