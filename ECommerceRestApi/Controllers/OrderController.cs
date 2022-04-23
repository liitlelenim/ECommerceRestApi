using ECommerceRestApi.DTO.Order;
using ECommerceRestApi.Entities;
using ECommerceRestApi.Repositories;
using ECommerceRestApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceRestApi.Controllers;

[ApiController]
[Route("api")]
public class OrderController : ControllerBase
{
    private readonly IOrderingService _orderingService;
    private readonly IOrderRepository _orderRepository;
    private readonly IAppUserRepository _userRepository;

    public OrderController(IOrderingService orderingService, IOrderRepository orderRepository,
        IAppUserRepository userRepository)
    {
        _orderingService = orderingService;
        _orderRepository = orderRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Creates order using user's cart.
    /// </summary>
    /// <param name="userNote"></param>
    [Authorize]
    [HttpPost, Route("orders")]
    [ProducesResponseType(typeof(GetOrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MakeOrder([FromBody] string userNote = "")
    {
        string username = HttpContext.User.FindFirst("username")!.Value;
        try
        {
            return Created($"/api/orders/{username}", await _orderingService.MakeOrder(username, userNote));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
    /// <summary>
    /// Returns history of all orders to owner.
    /// </summary>
    [Authorize(Roles = nameof(UserRole.Owner))]
    [HttpGet, Route("manage/orders")]
    [ProducesResponseType(typeof(IEnumerable<GetOrderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOrders()
    {
        return Ok(await _orderRepository.GetAllAsync());
    }
    /// <summary>
    /// Returns orders of given user but only if orders belong to requesting user.
    /// </summary>
    /// <param name="username"></param>
    [Authorize]
    [HttpGet, Route("orders/{username}")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(IEnumerable<GetOrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserOrders(string username)
    {
        if (username != HttpContext.User.FindFirst("username")!.Value)
        {
            return Forbid();
        }

        try
        {
            return Ok(await _orderRepository.GetUserOrdersAsync(await _userRepository.GetByUsernameAsync(username)));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}