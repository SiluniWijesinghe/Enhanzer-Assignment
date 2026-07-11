using System.ComponentModel.DataAnnotations;

namespace EnhanzerApi.DTOs;

public class PurchaseBillRequestDto
{
    [Required, MinLength(1, ErrorMessage = "At least one line item is required.")]
    public List<PurchaseBillLineDto> Lines { get; set; } = new();
}