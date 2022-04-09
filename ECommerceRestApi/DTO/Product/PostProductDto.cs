using System.ComponentModel.DataAnnotations;

namespace ECommerceRestApi.DTO.Product;

public class PostProductDto
{
    [StringLength(256)] public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public decimal Price { get; set; }
    public bool Availability { get; set; } = true;
}