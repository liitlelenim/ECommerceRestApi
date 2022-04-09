using ECommerceRestApi.Entities;

namespace ECommerceRestApi.DAL;

public class DebugDataSeeder
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public DebugDataSeeder(IConfiguration configuration, AppDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public void Seed()
    {
        AppUser? owner = _context.AppUsers.FirstOrDefault(u => u.Role == UserRole.Owner);
        if (owner is null)
        {
            _context.AppUsers.Add(new AppUser
            {
                Username = _configuration["Owner:Username"],
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(_configuration["Owner:Password"]),
                Role = UserRole.Owner
            });
            _context.SaveChanges();
        }
    }
}