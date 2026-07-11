using System.ComponentModel.DataAnnotations;

namespace EnhanzerApi.DTOs;

// What we send back to Angular after a successful login
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public string Email { get; set; } = string.Empty;
    public List<UserLocationDto> Locations { get; set; } = new();
}
