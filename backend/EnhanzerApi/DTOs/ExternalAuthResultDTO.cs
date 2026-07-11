namespace EnhanzerApi.DTOs;

public class ExternalAuthResultDTO
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public List<UserLocationDto> Locations { get; set; } = new();
}