using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnhanzerApi.Models;


[Table("Location_Details")]
public class LocationDetail
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string CompanyCode { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Location_Code { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Location_Name { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
