using ECommerceRestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceRestApi.DAL;

public class AppDbContext : DbContext
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Product> Products { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}