using ECommerceRestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceRestApi.DAL;

public class AppDbContext : DbContext
{
    public DbSet<AppUser> AppUsers { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}