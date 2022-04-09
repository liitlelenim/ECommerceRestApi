namespace ECommerceRestApi.DTO.Product;

public class GetProductDto
{
    public string Uri { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public decimal Price { get; set; }
}