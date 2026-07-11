using System.ComponentModel.DataAnnotations;

namespace EnhanzerApi.DTOs;
public class PurchaseBillResponseDto
{
    public int PurchaseBillId { get; set; }
    public List<PurchaseBillLineDto> Lines { get; set; } = new();
    public int TotalItems { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalCostSum { get; set; }
    public decimal TotalSellingSum { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
