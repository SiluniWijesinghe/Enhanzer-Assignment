using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EnhanzerApi.DTOs;
// Shape of one entry in the external API's User_Locations array
public class UserLocationDto
{
    // Explicit JSON names: keeps the exact casing from the external API
    // contract instead of letting ASP.NET Core's default camelCase
    // policy mangle the underscore in "Location_Code" / "Location_Name".
    [JsonPropertyName("Location_Code")]
    public string Location_Code { get; set; } = string.Empty;

    [JsonPropertyName("Location_Name")]
    public string Location_Name { get; set; } = string.Empty;
}
