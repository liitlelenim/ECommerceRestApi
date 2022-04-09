using System;
using System.Threading.Tasks;
using ECommerceRestApi.Controllers;
using ECommerceRestApi.DTO.Authentication;
using ECommerceRestApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ECommerceRestApiTests.Controllers;

public class AuthenticationControllerTests
{
    [Fact]
    public async Task Should_ReturnNoContent_When_RegistrationIsSuccessful()
    {
        var authServiceStub = new Mock<IAuthenticationService>();
        var authController = new AuthenticationController(authServiceStub.Object);

        var dtoStub = new UserRegistrationDto()
            { Username = "ValidUsername", Password = "ValidPassword", PasswordConfirmation = "ValidPassword" };

      (await authController.RegisterAccount(dtoStub)).Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Should_ReturnNoContent_When_RegistrationDtoIsInvalid()
    {
        var authServiceStub = new Mock<IAuthenticationService>();
        authServiceStub.Setup(stub => stub.Register(It.IsAny<UserRegistrationDto>()))
            .Throws<ArgumentException>();
        var authController = new AuthenticationController(authServiceStub.Object);
        var dtoStub = new UserRegistrationDto()
            { Username = "InValidUsername", Password = "ValidPassword", PasswordConfirmation = "ValidPassword" };

        (await authController.RegisterAccount(dtoStub)).Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Should_ReturnOkObject_When_LoginIsSuccessful()
    {
        var authServiceStub = new Mock<IAuthenticationService>();
        var authController = new AuthenticationController(authServiceStub.Object);

        var dtoStub = new UserLoginDto { Username = "ValidUsername", Password = "ValidPassword" };

        (await authController.Login(dtoStub)).Should().BeOfType<OkObjectResult>();
    }
    [Fact]
    public async Task Should_ReturnBadRequest_When_LoginFail()
    {
        var authServiceStub = new Mock<IAuthenticationService>();
        authServiceStub.Setup(stub => stub.Login(It.IsAny<UserLoginDto>()))
            .Throws<ArgumentException>();
        var authController = new AuthenticationController(authServiceStub.Object);
        var dtoStub = new UserLoginDto { Username = "InValidUsername", Password = "ValidPassword" };

        (await authController.Login(dtoStub)).Should().BeOfType<BadRequestObjectResult>();
    }
}