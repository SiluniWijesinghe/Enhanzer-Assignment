using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EnhanzerApi.Services;

public interface ITokenService
{
    (string token, DateTime expiresAtUtc) GenerateToken(string email);
}
