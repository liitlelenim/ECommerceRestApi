using ECommerceRestApi.DTO.Product;
using ECommerceRestApi.Entities;
using ECommerceRestApi.Repositories;

namespace ECommerceRestApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<GetProductDto>> GetAvailableProductsByPage(int page)
    {
        return (await _productRepository.GetAvailableByPageAsync(page))
            .Select(product => new GetProductDto
            {
                Uri = product.Uri,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            });
    }

    public async Task<Product> AddNewProduct(PostProductDto productDto)
    {
        Product product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Available = productDto.Availability,
            Price = productDto.Price
        };
        await _productRepository.InsertAsync(product);
        await _productRepository.SaveChangesAsync();
        return product;
    }

    public async Task UpdateProduct(int id, PostProductDto productDto)
    {
        Product productToUpdate = await _productRepository.GetByIdAsync(id);
        productToUpdate.Name = productDto.Name;
        productToUpdate.Description = productDto.Description;
        productToUpdate.Available = productDto.Availability;
        productToUpdate.Price = productDto.Price;
        await _productRepository.SaveChangesAsync();
    }
}

public interface IProductService
{
    public Task<IEnumerable<GetProductDto>> GetAvailableProductsByPage(int page);
    public Task<Product> AddNewProduct(PostProductDto productDto);
    public Task UpdateProduct(int id, PostProductDto productDto);
}