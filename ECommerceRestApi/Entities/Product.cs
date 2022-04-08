using System.ComponentModel.DataAnnotations;
using CSharpVitamins;

namespace ECommerceRestApi.Entities;

public class Product
{
    [Key] public int Id { get; set; }
    public string Uri { get; set; } = ShortGuid.NewGuid();
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}