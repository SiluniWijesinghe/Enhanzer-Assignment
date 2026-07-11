using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnhanzerApi.Models;

[Table("PurchaseBillLines")]
public class PurchaseBillLine
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(PurchaseBillHeader))]
    public int PurchaseBillHeaderId { get; set; }
    public PurchaseBillHeader? PurchaseBillHeader { get; set; }

    [Required, MaxLength(100)]
    public string Item { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Batch { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal StandardCost { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal StandardPrice { get; set; }

    public int Qty { get; set; }
    public int FreeQty { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal Discount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCost { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalSelling { get; set; }
}
