using System.ComponentModel.DataAnnotations;

namespace ECommerceRestApi.DTO.Authentication;

public class UserLoginDto
{
    [StringLength(255)] public string Username { get; set; } = String.Empty;
    [StringLength(255)] public string Password { get; set; } = String.Empty;
}