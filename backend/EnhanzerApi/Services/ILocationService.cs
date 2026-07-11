using EnhanzerApi.DTOs;

namespace EnhanzerApi.Services;

public interface ILocationService
{
    Task SaveLocationsAsync(string companyCode, List<UserLocationDto> locations);
    Task<List<UserLocationDto>> GetLocationsAsync(string companyCode);
}
