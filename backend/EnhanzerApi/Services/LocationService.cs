using EnhanzerApi.Data;
using EnhanzerApi.DTOs;
using EnhanzerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EnhanzerApi.Services;

public class LocationService : ILocationService
{
    private readonly AppDbContext _db;

    public LocationService(AppDbContext db)
    {
        _db = db;
    }

    public async Task SaveLocationsAsync(string companyCode, List<UserLocationDto> locations)
    {
        var existing = _db.LocationDetails.Where(l => l.CompanyCode == companyCode);
        _db.LocationDetails.RemoveRange(existing);

        var newRows = locations.Select(l => new LocationDetail
        {
            CompanyCode = companyCode,
            Location_Code = l.Location_Code,
            Location_Name = l.Location_Name
        });

        await _db.LocationDetails.AddRangeAsync(newRows);
        await _db.SaveChangesAsync();
    }

    public async Task<List<UserLocationDto>> GetLocationsAsync(string companyCode)
    {
        return await _db.LocationDetails
            .Where(l => l.CompanyCode == companyCode)
            .OrderBy(l => l.Location_Name)
            .Select(l => new UserLocationDto
            {
                Location_Code = l.Location_Code,
                Location_Name = l.Location_Name
            })
            .ToListAsync();
    }
}
