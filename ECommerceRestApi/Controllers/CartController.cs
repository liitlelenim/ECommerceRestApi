using ECommerceRestApi.DTO.CartItem;
using ECommerceRestApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ECommerceRestApi.Controllers;

[ApiController]
[Route("/api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }
    /// <summary>
    /// Adds item to your cart based on given dto.
    /// </summary>
    /// <param name="postCartItemDto"></param>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> AddToUserCart([FromBody] PostCartItemDto postCartItemDto)
    {
        try
        {
            string username = HttpContext.User!.FindFirst("username")!.Value;
            await _cartService.AddItemToCart(username, postCartItemDto.ProductUri, postCartItemDto.Quantity);
            return NoContent();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
    /// <summary>
    /// Removes item from your cart based on given dto.
    /// </summary>
    /// <param name="postCartItemDto"></param>
    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> RemoveFromUserCart([FromBody] PostCartItemDto postCartItemDto)
    {
        try
        {
            string username = HttpContext.User!.FindFirst("username")!.Value;
            await _cartService.RemoveItemFromCart(username, postCartItemDto.ProductUri, postCartItemDto.Quantity);
            return NoContent();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    /// <summary>
    /// Gets current content of your cart.
    /// </summary>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<GetCartItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserCart()
    {
        string username = HttpContext.User!.FindFirst("username")!.Value;
        return Ok(await _cartService.GetCartItems(username));
    }
}