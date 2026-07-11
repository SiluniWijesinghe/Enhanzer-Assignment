using System.Text;
using System.Text.Json;
using EnhanzerApi.DTOs;

namespace EnhanzerApi.Services;
public class PosApiResponse
{
    public int Status_Code { get; set; }
    public string? Message { get; set; }
    public List<PosResponseBodyItem> Response_Body { get; set; } = new();
}

public class PosResponseBodyItem
{
    public string? Email { get; set; }
    public string? Doc_Msg { get; set; }
    public List<UserLocationDto>? User_Locations { get; set; }
}

public class ExternalAuthResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public List<UserLocationDto> Locations { get; set; } = new();
}

public class ExternalAuthService : IExternalAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<ExternalAuthService> _logger;

    public ExternalAuthService(HttpClient httpClient, IConfiguration config, ILogger<ExternalAuthService> logger)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }

   public async Task<ExternalAuthResult> LoginAsync(string email, string password)
    {
        
        var endpoint = _config["ExternalApi:LoginEndpoint"]!;
        var deviceId = _config["ExternalApi:DeviceId"] ?? "D001";

        var payload = new
        {
            API_Action = "GetLoginData",
            Device_Id = deviceId,
            Sync_Time = "",
            Company_Code = email,
            API_Body = new { Username = email, Pw = password }
        };

        HttpResponseMessage response;
        try
        {
            _logger.LogWarning("OUTGOING REQUEST PAYLOAD: {Json}", JsonSerializer.Serialize(payload));
            response = await _httpClient.PostAsJsonAsync(endpoint, payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "External login API call failed");
            return new ExternalAuthResult { Success = false, ErrorMessage = "Unable to reach the authentication service. Please try again." };
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("External API returned {StatusCode}: {Body}", response.StatusCode, errorBody);
            return new ExternalAuthResult { Success = false, ErrorMessage = "Invalid email or password." };
        }

        var rawJson = await response.Content.ReadAsStringAsync();
        _logger.LogWarning("RAW EXTERNAL API RESPONSE: {Json}", rawJson);

        var result = JsonSerializer.Deserialize<PosApiResponse>(rawJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        var body = result?.Response_Body?.FirstOrDefault();

        if (body?.User_Locations is not null)
            return new ExternalAuthResult { Success = true, Locations = body.User_Locations };

        return new ExternalAuthResult { Success = false, ErrorMessage = body?.Doc_Msg ?? "Invalid email or password." };
    }

    
   
}