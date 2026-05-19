using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Shop.Models;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Shop.Services;

/// <summary>
/// Serwis tworzenia zamówień. Generuje numer KNK-O-XXXX-XXXXX i zapisuje zamówienie
/// razem z pozycjami (snapshot nazwy i ceny szablonu).
///
/// ADO.NET UPSERT na sequence_counters — identyczny wzorzec jak Rental i Decoration.
/// </summary>
public class OrderService(AppDbContext db)
{
    public async Task<string> CreateOrderAsync(
        OrderFormModel model,
        CancellationToken ct = default)
    {
        // Wczytaj wybrany szablon — snapshot nazwy i ceny
        var template = await db.Set<BouquetTemplate>()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == model.TemplateId!.Value && t.IsActive, ct)
            ?? throw new InvalidOperationException("Wybrany szablon bukietu nie istnieje lub jest nieaktywny.");

        var year = DateTime.UtcNow.Year;
        var sequenceNumber = await GenerateSequenceNumberAsync("O", year, ct);
        var orderNumber = $"KNK-O-{year}-{sequenceNumber:D5}";

        var subtotal = template.BasePrice * model.Quantity;
        var deliveryFee = model.DeliveryMethod switch
        {
            DeliveryMethod.Pickup        => 0m,
            DeliveryMethod.LocalDelivery => 15m,
            DeliveryMethod.Courier       => 25m,
            _                            => 0m
        };

        var order = new Order
        {
            OrderNumber       = orderNumber,
            Status            = OrderStatus.New,
            CustomerName      = model.CustomerName,
            CustomerEmail     = model.CustomerEmail,
            CustomerPhone     = model.CustomerPhone,
            DeliveryMethod    = model.DeliveryMethod,
            DeliveryAddress   = model.DeliveryAddress,
            DeliveryDate      = model.DeliveryDate,
            CustomerNote      = model.CustomerNote,
            Subtotal          = subtotal,
            DeliveryFee       = deliveryFee,
            TotalPrice        = subtotal + deliveryFee,
            PaymentStatus     = PaymentStatus.Unpaid,
            Items =
            [
                new OrderItem
                {
                    BouquetTemplateId     = template.Id,
                    BouquetNameSnapshot   = template.Name,
                    Quantity              = model.Quantity,
                    UnitPriceSnapshot     = template.BasePrice
                }
            ]
        };

        db.Set<Order>().Add(order);
        await db.SaveChangesAsync(ct);

        return orderNumber;
    }

    // Identyczny mechanizm jak w RentalReservationService i DecorationInquiryService.
    // ADO.NET UPSERT zamiast EF SqlQuery — bo INSERT...RETURNING nie jest composable.
    private async Task<int> GenerateSequenceNumberAsync(string type, int year, CancellationToken ct)
    {
        var connection = db.Database.GetDbConnection();
        var shouldClose = connection.State != System.Data.ConnectionState.Open;
        if (shouldClose) await connection.OpenAsync(ct);

        try
        {
            await using var cmd = connection.CreateCommand();
            cmd.CommandText = """
                INSERT INTO sequence_counters (type, year, last_value)
                VALUES (@type, @year, 1)
                ON CONFLICT (type, year) DO UPDATE
                    SET last_value = sequence_counters.last_value + 1
                RETURNING last_value
                """;

            var pType = cmd.CreateParameter();
            pType.ParameterName = "type";
            pType.Value = type;
            cmd.Parameters.Add(pType);

            var pYear = cmd.CreateParameter();
            pYear.ParameterName = "year";
            pYear.Value = year;
            cmd.Parameters.Add(pYear);

            var scalar = await cmd.ExecuteScalarAsync(ct);
            return Convert.ToInt32(scalar);
        }
        finally
        {
            if (shouldClose) await connection.CloseAsync();
        }
    }
}
