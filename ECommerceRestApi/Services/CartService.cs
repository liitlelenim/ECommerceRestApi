using ECommerceRestApi.DTO;
using ECommerceRestApi.DTO.CartItem;
using ECommerceRestApi.DTO.Order;
using ECommerceRestApi.Entities;
using ECommerceRestApi.Repositories;

namespace ECommerceRestApi.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<GetCartItemDto>> GetCartItems(string username)
    {
        Cart userCart = await _cartRepository.GetByUserUsernameAsync(username);
        return userCart.Items.Select(item => new GetCartItemDto
        {
            Quantity = item.Quantity,
            Uri = item.Product.Uri,
            Name = item.Product.Name,
            Description = item.Product.Description,
            Price = item.Product.Price
        });
    }

    public async Task RemoveItemFromCart(string username, string productUri, int quantityToRemove)
    {
        if (quantityToRemove <= 0)
        {
            throw new ArgumentException("Quantity must be a positive integer.");
        }

        Cart userCart = await _cartRepository.GetByUserUsernameAsync(username);
        CartItem? itemToEdit = userCart.Items.FirstOrDefault(item => item.Product.Uri == productUri);
        if (itemToEdit is null)
        {
            throw new ArgumentException("You do not have item with given uri in your cart.");
        }

        if (itemToEdit.Quantity - quantityToRemove > 0)
        {
            itemToEdit.Quantity -= quantityToRemove;
        }
        else
        {
            userCart.Items.Remove(itemToEdit);
        }

        await _cartRepository.SaveAsync();
    }

    public async Task AddItemToCart(string username, string productUri, int productQuantity)
    {
        if (productQuantity <= 0)
        {
            throw new ArgumentException("Quantity must be a positive integer.");
        }

        Cart userCart = await _cartRepository.GetByUserUsernameAsync(username);
        CartItem? itemToEdit = userCart.Items.FirstOrDefault(item => item.Product.Uri == productUri);
        if (itemToEdit is null)
        {
            itemToEdit = new CartItem();
            itemToEdit.Product = await _productRepository.GetByUriAsync(productUri);
            if (!itemToEdit.Product.Available)
            {
                throw new ArgumentException("Product that you want to add is not available right now.");
            }

            itemToEdit.Quantity = productQuantity;
            userCart.Items.Add(itemToEdit);
        }
        else
        {
            itemToEdit.Quantity += productQuantity;
        }

        await _cartRepository.SaveAsync();
    }
}

public interface ICartService
{
    public Task<IEnumerable<GetCartItemDto>> GetCartItems(string username);
    public Task RemoveItemFromCart(string username, string productUri, int quantityToRemove);
    public Task AddItemToCart(string username, string productUri, int productQuantity);
}