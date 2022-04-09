using ECommerceRestApi.DAL;
using ECommerceRestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceRestApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAvailableByPageAsync(int pageIndex, int pageSize = 10)
    {
        return await _context.Products
            .Where(p =>p.Available)
            .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        Product? product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product is null)
        {
            throw new ArgumentException("Product with given id does not exist.");
        }

        return product;
    }

    public async Task<Product> GetByUriAsync(string uri)
    {
        Product? product = await _context.Products.FirstOrDefaultAsync(p => p.Uri == uri);
        if (product is null)
        {
            throw new ArgumentException("Product with given uri does not exist.");
        }

        return product;
    }

    public async Task InsertAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public void Remove(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

public interface IProductRepository
{
    public Task<IEnumerable<Product>> GetAllAsync();
    public Task<IEnumerable<Product>> GetAvailableByPageAsync(int pageIndex, int pageSize = 10);
    public Task<Product> GetByIdAsync(int id);
    public Task<Product> GetByUriAsync(string uri);
    public Task InsertAsync (Product product);
    public void Remove(Product product);
    public Task SaveChangesAsync();
}