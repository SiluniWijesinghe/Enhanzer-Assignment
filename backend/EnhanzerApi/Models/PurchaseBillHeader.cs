using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnhanzerApi.Models;

[Table("PurchaseBillHeaders")]
public class PurchaseBillHeader
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string CreatedByEmail { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<PurchaseBillLine> Lines { get; set; } = new List<PurchaseBillLine>();
}
