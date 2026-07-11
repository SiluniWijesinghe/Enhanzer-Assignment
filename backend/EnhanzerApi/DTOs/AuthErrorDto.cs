using System.ComponentModel.DataAnnotations;

namespace EnhanzerApi.DTOs;

// Generic problem payload returned on auth failure
public class AuthErrorDto
{
    public string Message { get; set; } = string.Empty;
}
