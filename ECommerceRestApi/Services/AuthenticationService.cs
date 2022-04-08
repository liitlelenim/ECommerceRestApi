using ECommerceRestApi.DTO.Authentication;
using ECommerceRestApi.Entities;
using ECommerceRestApi.Repositories;
using JWT.Algorithms;
using JWT.Builder;

namespace ECommerceRestApi.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly IAppUserRepository _appUserRepository;

    public AuthenticationService(IAppUserRepository appUserRepository, IConfiguration configuration)
    {
        _appUserRepository = appUserRepository;
        _configuration = configuration;
    }

    public async Task Register(UserRegistrationDto dto)
    {
        if (!dto.Password.Equals(dto.PasswordConfirmation))
        {
            throw new ArgumentException("Given passwords do not match.");
        }

        if (await _appUserRepository.ExistByUsernameAsync(dto.Username))
        {
            throw new ArgumentException("User with given username already exist.");
        }

        if (dto.Username.Length < 3)
        {
            throw new ArgumentException("Username must be at least 3 characters long.");
        }

        if (dto.Password.Length < 6)
        {
            throw new ArgumentException("Password must be at least 6 characters long.");
        }

        AppUser user = new AppUser
        {
            Username = dto.Username,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Username)
        };
        await _appUserRepository.InsertAsync(user);
        await _appUserRepository.SaveAsync();
    }

    public async Task<string> Login(UserLoginDto dto)
    {
        if (!await _appUserRepository.ExistByUsernameAsync(dto.Username))
        {
            throw new ArgumentException("Given username or password is incorrect.");
        }

        AppUser user = await _appUserRepository.GetByUsernameAsync(dto.Username);
        if (BCrypt.Net.BCrypt.Verify(dto.Password, user.HashedPassword))
        {
            return CreateJwt(user);
        }

        throw new ArgumentException("Given username or password is incorrect.");
    }

    public string CreateJwt(AppUser user)
    {
        return JwtBuilder.Create().WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(_configuration["Jwt:Secret"])
            .AddClaim("username", user.Username)
            .AddClaim("role", user.Role.ToString())
            .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
            .Encode();
    }
}

public interface IAuthenticationService
{
    public Task Register(UserRegistrationDto dto);
    public Task<string> Login(UserLoginDto dto);
    public string CreateJwt(AppUser user);
}