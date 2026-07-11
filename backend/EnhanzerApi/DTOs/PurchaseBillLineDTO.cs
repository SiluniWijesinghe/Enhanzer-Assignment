using System.ComponentModel.DataAnnotations;

namespace EnhanzerApi.DTOs;

public class PurchaseBillLineDto
{
    [Required]
    public string Item { get; set; } = string.Empty;

    [Required]
    public string Batch { get; set; } = string.Empty; 
    [Range(0, double.MaxValue)]
    public decimal StandardCost { get; set; }

    [Range(0, double.MaxValue)]
    public decimal StandardPrice { get; set; }

    [Range(1, int.MaxValue)]
    public int Qty { get; set; }

    [Range(0, int.MaxValue)]
    public int FreeQty { get; set; }

    [Range(0, 100)]
    public decimal Discount { get; set; } // percentage, e.g. 20 = 20%

    // Server recalculates these itself rather than trusting the client's numbers
    public decimal TotalCost { get; set; }
    public decimal TotalSelling { get; set; }
}
