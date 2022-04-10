using ECommerceRestApi.DTO.Product;
using ECommerceRestApi.Entities;
using ECommerceRestApi.Repositories;
using ECommerceRestApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ECommerceRestApi.Controllers;

[ApiController]
[Route("/api/manage/products")]
public class ProductManagementController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IProductService _productService;
    
    public ProductManagementController(IProductService productService, IProductRepository productRepository)
    {
        _productService = productService;
        _productRepository = productRepository;
    }

    /// <summary>
    /// Gives collection of every product entity if requesting user is owner.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = nameof(UserRole.Owner))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAllProductsEntity()
    {
        IEnumerable<Product> products = await _productRepository.GetAllAsync();
        if (products.Any())
        {
            return Ok(products);
        }
        return NoContent();
    }
    /// <summary>
    /// Creates a new product if requesting user is owner. 
    /// </summary>
    /// <param name="postProductDto"></param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Authorize(Roles = nameof(UserRole.Owner))]
    public async Task<IActionResult> PostProduct([FromBody] PostProductDto postProductDto)
    {
        var createdProduct = await _productService.AddNewProduct(postProductDto);
        return Created(createdProduct.Uri, createdProduct);
    }
    /// <summary>
    /// Updates a product with given id, using given dto.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="postProductDto"></param>
    [HttpPut, Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProduct(int id, PostProductDto postProductDto)
    {
        try
        {
            await _productService.UpdateProduct(id, postProductDto);
            return NoContent();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}