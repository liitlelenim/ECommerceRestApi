using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceRestApi.Controllers;
using ECommerceRestApi.Entities;
using ECommerceRestApi.Repositories;
using ECommerceRestApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ECommerceRestApiTests.Controllers;

public class ProductManagementTests
{
    [Fact]
    public async Task Should_ReturnNoContent_When_ThereAreNoProductsInDatabase()
    {
        var productRepoStub = new Mock<IProductRepository>();
        productRepoStub.Setup(repo => repo.GetAllAsync())
            .Returns(Task.FromResult(Enumerable.Empty<Product>()));

        var productServiceStub = new Mock<IProductService>();

        var controller = new ProductManagementController(productServiceStub.Object, productRepoStub.Object);
        (await controller.GetAllProductsEntity()).Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Should_ReturnEveryProductFromRepositoryAndBeOkObjectResult()
    {
        var products = new List<Product>()
        {
            new Product
            {
                Id = 1,
                Name = "Stub"
            },
            new Product
            {
                Id = 2,
                Name = "Stub",
                Description = "Stub",
                Price = 2.43m
            },
            new Product
            {
                Id = 3,
                Name = "Stub",
                Description = "Stub",
                Price = 2.43m
            }
        };

        var productRepoStub = new Mock<IProductRepository>();
        productRepoStub.Setup(repo => repo.GetAllAsync())
            .Returns(Task.FromResult(products.AsEnumerable()));

        var productServiceStub = new Mock<IProductService>();
        var controller = new ProductManagementController(productServiceStub.Object, productRepoStub.Object);

        var response = await controller.GetAllProductsEntity();
        (response as OkObjectResult)?.Value.Should().BeEquivalentTo(products);
    }
    
}