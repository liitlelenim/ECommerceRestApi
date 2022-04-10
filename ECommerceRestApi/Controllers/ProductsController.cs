using ECommerceRestApi.DTO.Product;
using ECommerceRestApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ECommerceRestApi.Controllers;

[ApiController]
[Route("/api/products/")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    /// <summary>
    /// Gives user a paginated available products dto
    /// </summary>
    /// <param name="pageIndex"></param>
    [HttpGet, Route("page/{pageIndex:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPageOfProducts(int pageIndex = 1)
    {
        if (pageIndex < 1)
        {
            return BadRequest("Page index must be at least 1");
        }
        IEnumerable<GetProductDto> productsDtoPage = await _productService.GetAvailableProductsByPage(pageIndex);
        if (productsDtoPage.Any())
        {
            return Ok(productsDtoPage);
        }

        return NoContent();
    }
}