namespace EnhanzerApi.Services;

public interface IExternalAuthService
{
    Task<ExternalAuthResult> LoginAsync(string email, string password);
}