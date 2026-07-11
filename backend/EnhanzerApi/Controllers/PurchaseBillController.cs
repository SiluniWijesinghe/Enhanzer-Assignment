using System.Security.Claims;
using EnhanzerApi.Data;
using EnhanzerApi.DTOs;
using EnhanzerApi.Models;
using EnhanzerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnhanzerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseBillController : ControllerBase
{
    private readonly AppDbContext _db;

    private static readonly string[] Items =
    {
        "Mango", "Apple", "Banana", "Orange", "Grapes", "Kiwi", "Strawberry"
    };

    public PurchaseBillController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("items")]
    public IActionResult GetItems() => Ok(Items);

    [HttpPost]
    public async Task<ActionResult<PurchaseBillResponseDto>> Create([FromBody] PurchaseBillRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var email = User.FindFirstValue(ClaimTypes.Email) ?? "unknown";

        var header = new PurchaseBillHeader
        {
            CreatedByEmail = email
        };

        foreach (var line in request.Lines)
        {
           
            PurchaseBillCalculator.Recalculate(line);

            header.Lines.Add(new PurchaseBillLine
            {
                Item = line.Item,
                Batch = line.Batch,
                StandardCost = line.StandardCost,
                StandardPrice = line.StandardPrice,
                Qty = line.Qty,
                FreeQty = line.FreeQty,
                Discount = line.Discount,
                TotalCost = line.TotalCost,
                TotalSelling = line.TotalSelling
            });
        }

        _db.PurchaseBillHeaders.Add(header);
        await _db.SaveChangesAsync();

        var response = new PurchaseBillResponseDto
        {
            PurchaseBillId = header.Id,
            Lines = request.Lines,
            TotalItems = request.Lines.Count,
            TotalQuantity = request.Lines.Sum(l => l.Qty),
            TotalCostSum = request.Lines.Sum(l => l.TotalCost),
            TotalSellingSum = request.Lines.Sum(l => l.TotalSelling),
            CreatedAtUtc = header.CreatedAtUtc
        };

        return CreatedAtAction(nameof(Create), new { id = header.Id }, response);
    }
}
