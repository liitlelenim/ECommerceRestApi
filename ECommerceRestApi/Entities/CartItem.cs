using System.ComponentModel.DataAnnotations;

namespace ECommerceRestApi.Entities;

public class CartItem
{
    [Key] public int Id { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
}