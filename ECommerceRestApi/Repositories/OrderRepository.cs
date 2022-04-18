using ECommerceRestApi.DAL;
using ECommerceRestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceRestApi.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(AppUser user)
    {
        _context.AppUsers.Include(u => u.Orders)
            .ThenInclude(order => order.OrderedItems)
            .ThenInclude(item => item.Product);
        return await _context.Orders.Where(order => order.Customer == user).ToListAsync();
    }

    public async Task<Order> GetByIdAsync(int id)
    {
        _context.AppUsers.Include(u => u.Orders)
            .ThenInclude(order => order.OrderedItems)
            .ThenInclude(item => item.Product);
        Order? order = await _context.Orders.FirstOrDefaultAsync(order => order.Id == id);
        if (order is null)
        {
            throw new ArgumentException("Order with given id does not exist.");
        }

        return order;
    }

    public void Remove(Order order)
    {
        _context.Orders.Remove(order);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}

public interface IOrderRepository
{
    public Task<IEnumerable<Order>> GetAllAsync();
    public Task<IEnumerable<Order>> GetUserOrdersAsync(AppUser user);
    public Task<Order> GetByIdAsync(int id);
    public void Remove(Order order);
    public Task SaveAsync();
}