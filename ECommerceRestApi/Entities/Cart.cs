using System.ComponentModel.DataAnnotations;

namespace ECommerceRestApi.Entities;

public class Cart
{
    [Key] public int Id { get; set; }
    public AppUser Owner { get; set; }  = null!;
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
