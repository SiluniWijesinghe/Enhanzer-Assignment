using EnhanzerApi.DTOs;

namespace EnhanzerApi.Services;

public interface IExternalAuthService
{
    Task<ExternalAuthResultDTO> LoginAsync(string email, string password);
}