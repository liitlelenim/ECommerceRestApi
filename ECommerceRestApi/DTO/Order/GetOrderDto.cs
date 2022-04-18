using ECommerceRestApi.DTO.CartItem;

namespace ECommerceRestApi.DTO.Order;

public class GetOrderDto
{
    public string CustomerNote { get; set; } = String.Empty;
    public string CustomerName { get; set; } = String.Empty;
    public decimal FinalPrice { get; set; }
    public DateTime Timestamp { get; set; }
    public IEnumerable<GetCartItemDto> OrderedItems { get; set; } = Enumerable.Empty<GetCartItemDto>();
}