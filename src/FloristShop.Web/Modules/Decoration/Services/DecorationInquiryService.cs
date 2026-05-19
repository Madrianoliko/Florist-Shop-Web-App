using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Decoration.Models;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Decoration.Services;

/// <summary>
/// Serwis obsługi zapytań o wycenę dekoracji. Tworzy zapytania od klientów.
/// Generuje numer KNK-D-XXXX-XXXXX identycznym mechanizmem jak moduł Rental
/// (ADO.NET UPSERT na tabeli sequence_counters).
/// </summary>
public class DecorationInquiryService(AppDbContext db)
{
    public async Task<string> CreateInquiryAsync(
        DecorationInquiryFormModel model,
        CancellationToken ct = default)
    {
        var year = DateTime.UtcNow.Year;
        var sequenceNumber = await GenerateSequenceNumberAsync("D", year, ct);
        var inquiryNumber = $"KNK-D-{year}-{sequenceNumber:D5}";

        var inquiry = new DecorationInquiry
        {
            InquiryNumber = inquiryNumber,
            EventType = model.EventType,
            EventDate = model.EventDate,
            Location = model.Location,
            Description = model.Description,
            BudgetEstimate = model.BudgetEstimate,
            CustomerName = model.CustomerName,
            CustomerEmail = model.CustomerEmail,
            CustomerPhone = model.CustomerPhone,
            Status = DecorationInquiryStatus.New
        };

        db.Set<DecorationInquiry>().Add(inquiry);
        await db.SaveChangesAsync(ct);

        return inquiryNumber;
    }

    // Identyczny mechanizm jak w RentalReservationService — ADO.NET zamiast
    // db.Database.SqlQuery<int> (które nie działa z INSERT...RETURNING).
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
