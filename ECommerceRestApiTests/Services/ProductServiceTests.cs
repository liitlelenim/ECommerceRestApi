using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceRestApi.DTO.Product;
using ECommerceRestApi.Entities;
using ECommerceRestApi.Repositories;
using ECommerceRestApi.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace ECommerceRestApiTests.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task Should_ReturnProperDtos_When_GettingProductByPage()
    {
        IEnumerable<Product> products = new List<Product>()
        {
            new Product()
            {
                Name = "Name",
            },
            new Product()
            {
                Name = "Name",
                Description = "Description"
            },
            new Product()
            {
                Name = "Name",
                Description = "Description",
                Price = 25.99m
            },
        };
        IEnumerable<GetProductDto> getProductDtos = new List<GetProductDto>()
        {
            new GetProductDto
            {
                Name = "Name"
            },
            new GetProductDto
            {
                Name = "Name",
                Description = "Description"
            },
            new GetProductDto
            {
                Name = "Name",
                Description = "Description",
                Price = 25.99m
            },
        };

        var productRepoStub = new Mock<IProductRepository>();
        productRepoStub.Setup(
                repo => repo.GetAvailableByPageAsync(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task.FromResult(products));
        var productService = new ProductService(productRepoStub.Object);

        (await productService.GetAvailableProductsByPage(1))
            .Should()
            .BeEquivalentTo(getProductDtos, options => options.Excluding(dto => dto.Uri));
    }

    [Fact]
    public async Task Should_UpdateProductProperly_When_ProperIdAndDtoIsGiven()
    {
        var productToUpdate = new Product
        {
            Id = 1,
            Name = "BeforeUpdate"
        };
        var updateDto = new PostProductDto
        {
            Name = "AfterUpdate"
        };
        
        var productRepoStub = new Mock<IProductRepository>();
        productRepoStub.Setup(repo => repo.GetByIdAsync(1)).Returns(Task.FromResult(productToUpdate));
        
        var productService = new ProductService(productRepoStub.Object);

        await productService.UpdateProduct(productToUpdate.Id, updateDto);

        productToUpdate.Name.Should().Be(updateDto.Name);
    }
}