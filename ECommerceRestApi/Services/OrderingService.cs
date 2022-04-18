using ECommerceRestApi.DAL;
using ECommerceRestApi.DTO.CartItem;
using ECommerceRestApi.DTO.Order;
using ECommerceRestApi.Entities;
using ECommerceRestApi.Repositories;

namespace ECommerceRestApi.Services;

public class OrderingService : IOrderingService
{
    private readonly AppDbContext _dbContext;
    private readonly IOrderRepository _orderRepository;
    private readonly IAppUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;

    public OrderingService(IOrderRepository orderRepository, ICartRepository cartRepository,
        IAppUserRepository userRepository, AppDbContext dbContext)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _userRepository = userRepository;
        _dbContext = dbContext;
    }

    public async Task<GetOrderDto> MakeOrder(string username, string userNote)
    {
        
        AppUser orderingUser = await _userRepository.GetByUsernameAsync(username);
        Cart userCart = await _cartRepository.GetByUserUsernameAsync(username);
        IEnumerable<CartItem> cartItems = userCart.Items;
        if (!cartItems.Any())
        {
            throw new ArgumentException("Cart of this user is currently empty.");
        }

        decimal finalPrice = cartItems.Sum(item => item.Product.Price);
        Order order = new Order()
        {
            Customer = orderingUser,
            CustomerNote = username,
            FinalPrice = finalPrice,
            OrderedItems = cartItems.ToList()
        };
        userCart.Items.Clear();
        await _orderRepository.SaveAsync();
        return new GetOrderDto()
        {
            CustomerName = order.Customer.Username,
            CustomerNote = userNote,
            FinalPrice = order.FinalPrice,
            Timestamp = order.Timestamp,
            OrderedItems = order.OrderedItems.Select(item => new GetCartItemDto
            {
                Quantity = item.Quantity,
                Uri = item.Product.Uri,
                Name = item.Product.Name,
                Description = item.Product.Description,
                Price = item.Product.Price
            })
        };
    }
}

public interface IOrderingService
{
    public Task<GetOrderDto> MakeOrder(string username, string userNote);
}