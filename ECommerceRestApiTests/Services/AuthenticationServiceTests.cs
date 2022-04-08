using System;
using System.Threading.Tasks;
using ECommerceRestApi.DTO.Authentication;
using ECommerceRestApi.Entities;
using ECommerceRestApi.Repositories;
using ECommerceRestApi.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace ECommerceRestApiTests.Services;

public class AuthenticationServiceTests
{
    [Fact]
    public async Task Should_ThrowArgumentException_When_InvalidRegistrationDtoIsGiven()
    {
        var appUserRepoStub = new Mock<IAppUserRepository>();
        var configurationStub = new Mock<IConfiguration>();

        var authService = new AuthenticationService(appUserRepoStub.Object, configurationStub.Object);

        var registrationDto = new[]
        {
            new UserRegistrationDto
                { Username = "sh", Password = "ValidPassword", PasswordConfirmation = "ValidPassword" },
            new UserRegistrationDto { Username = "ValidUsername", Password = "short", PasswordConfirmation = "short" },
            new UserRegistrationDto
                { Username = "ValidUsername", Password = "ValidPassword", PasswordConfirmation = "MismatchedPassword" }
        };

        foreach (var dto in registrationDto)
        {
            Func<Task> registrationAction = async () => await authService.Register(dto);
            await registrationAction.Should().ThrowExactlyAsync<ArgumentException>();
        }
    }

    [Fact]
    public async Task Should_ThrowArgumentException_When_UserAlreadyExist()
    {
        var appUserRepoStub = new Mock<IAppUserRepository>();
        appUserRepoStub.Setup(repo => repo.ExistByUsernameAsync(It.IsAny<string>())).Returns(Task.FromResult(true));
        var configurationStub = new Mock<IConfiguration>();

        var authService = new AuthenticationService(appUserRepoStub.Object, configurationStub.Object);

        Func<Task> registrationAction = async () => await authService.Register(
            new UserRegistrationDto
            {
                Username = "ValidUsername",
                Password = "ValidPassword",
                PasswordConfirmation = "ValidPassword"
            }
        );
        await registrationAction.Should().ThrowExactlyAsync<ArgumentException>();
    }

    [Fact]
    public async Task Should_ThrowArgumentException_When_LoginUsernameDoNotMatch()
    {
        var appUserRepoStub = new Mock<IAppUserRepository>();
        appUserRepoStub.Setup(repo => repo.ExistByUsernameAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
        var configurationStub = new Mock<IConfiguration>();

        var authService = new AuthenticationService(appUserRepoStub.Object, configurationStub.Object);
        var invalidUsernameLoginDto = new UserLoginDto { Username = "InvalidUsername", Password = "ValidPassword" };
        Func<Task> loginAction = async () => await authService.Login(invalidUsernameLoginDto);
        await loginAction.Should().ThrowExactlyAsync<ArgumentException>();
    }
}