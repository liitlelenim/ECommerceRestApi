using Microsoft.EntityFrameworkCore;

namespace ECommerceRestApi.DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}