namespace ECommerceRestApi.DTO.CartItem;

public class PostCartItemDto
{
    public  int Quantity { get; set; }
    public string ProductUri { get; set; } = String.Empty;
}