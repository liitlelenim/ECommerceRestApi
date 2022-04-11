namespace ECommerceRestApi.DTO.CartItem;

public class GetCartItemDto
{
    public int Quantity { get; set; }
    public string Uri { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public decimal Price { get; set; }
}