using System.ComponentModel.DataAnnotations;

namespace ECommerceRestApi.Entities;

public class AppUser
{
    [Key] public int Id { get; set; }
    [StringLength(255)] public string Username { get; set; } = String.Empty;
    public string HashedPassword { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public UserRole Role { get; set; } = UserRole.Customer;

    public ICollection<Order> Orders { get; set; } = null!;
}

public enum UserRole
{
    Customer,
    Owner
}