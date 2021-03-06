using System.ComponentModel.DataAnnotations;

namespace ECommerceRestApi.Entities;

public class Order
{
    [Key] public int Id { get; set; }
    [StringLength(1024)] public string CustomerNote { get; set; } = String.Empty;
    public decimal FinalPrice { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;

    public IEnumerable<CartItem> OrderedItems { get; set; } = null!;
    public AppUser Customer { get; set; } = null!;
}