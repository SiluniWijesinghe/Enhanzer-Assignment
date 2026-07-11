using EnhanzerApi.DTOs;

namespace EnhanzerApi.Services;

public static class PurchaseBillCalculator
{
    public static void Recalculate(PurchaseBillLineDto line)
    {
        var gross = line.StandardCost * line.Qty;
        var discountAmount = gross * (line.Discount / 100m);
        line.TotalCost = Math.Round(gross - discountAmount, 2);
        line.TotalSelling = Math.Round(line.StandardPrice * line.Qty, 2);
    }
}
