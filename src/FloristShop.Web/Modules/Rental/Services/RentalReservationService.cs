using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Rental.Models;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Rental.Services;

/// <summary>
/// Serwis obsługi rezerwacji wypożyczalni.
/// Odpowiada za tworzenie zapytań rezerwacji i generowanie numerów trackingowych.
/// </summary>
public class RentalReservationService(AppDbContext db)
{
    /// <summary>
    /// Tworzy nową rezerwację na podstawie danych z formularza.
    /// Zwraca przydzielony numer trackingowy (np. "KNK-R-2026-00001").
    ///
    /// Rezerwacja startuje ze statusem Pending — admin zatwierdza ją po kontakcie
    /// z klientem i ewentualnym dorzuceniu pozycji (RentalReservationItems).
    ///
    /// TotalPrice = 0 przy tworzeniu przez klienta — admin ustala cenę przy potwierdzeniu.
    /// </summary>
    public async Task<string> CreateReservationAsync(ReservationFormModel model, CancellationToken ct = default)
    {
        var year = DateTime.UtcNow.Year;
        var sequenceNumber = await GenerateSequenceNumberAsync("R", year, ct);
        var reservationNumber = $"KNK-R-{year}-{sequenceNumber:D5}";

        var reservation = new RentalReservation
        {
            ReservationNumber = reservationNumber,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            CustomerName = model.CustomerName,
            CustomerEmail = model.CustomerEmail,
            CustomerPhone = model.CustomerPhone,
            CustomerNote = model.CustomerNote,
            Status = RentalReservationStatus.Pending,
            TotalPrice = 0m   // Ustala admin po weryfikacji dostępności i rozmowie z klientem
        };

        db.Set<RentalReservation>().Add(reservation);
        await db.SaveChangesAsync(ct);

        return reservationNumber;
    }

    /// <summary>
    /// Generuje kolejny numer sekwencji dla danego typu i roku.
    ///
    /// Używa PostgreSQL INSERT ... ON CONFLICT DO UPDATE ... RETURNING — jedna
    /// atomowa instrukcja SQL, bez żadnych blokad po stronie aplikacji.
    ///
    /// Dlaczego ADO.NET zamiast db.Database.SqlQuery&lt;int&gt;?
    /// EF Core's SqlQuery opakowuje podany SQL w SELECT * FROM (...).
    /// To działa dla SELECT-ów, ale INSERT...RETURNING to DML — nie jest
    /// "composable SQL" i EF rzuca InvalidOperationException przy próbie kompozycji.
    /// Bezpośredni DbCommand omija ten mechanizm całkowicie.
    ///
    /// Jak działa UPSERT:
    ///  1. Spróbuj INSERT nowego wiersza z last_value = 1 (pierwszy dokument roku).
    ///  2. Jeśli wiersz (type, year) już istnieje → ON CONFLICT: INCREMENT last_value.
    ///  3. RETURNING last_value → zwraca nową wartość w tej samej instrukcji.
    ///
    /// Postgres serializuje zapisy do jednego wiersza — dwa równoczesne wywołania
    /// ZAWSZE dostaną różne numery. Nie potrzebujemy transakcji aplikacyjnej.
    /// </summary>
    private async Task<int> GenerateSequenceNumberAsync(string type, int year, CancellationToken ct)
    {
        var connection = db.Database.GetDbConnection();

        // EF Core może już mieć otwarte połączenie (np. w ramach innej operacji).
        // Otwieramy sami tylko jeśli jest zamknięte — i wtedy też zamykamy po sobie.
        var shouldClose = connection.State != System.Data.ConnectionState.Open;
        if (shouldClose)
            await connection.OpenAsync(ct);

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

            // ExecuteScalarAsync zwraca pierwszą kolumnę pierwszego wiersza wyniku.
            // RETURNING last_value daje dokładnie jeden wiersz z jedną kolumną int.
            var scalar = await cmd.ExecuteScalarAsync(ct);
            return Convert.ToInt32(scalar);
        }
        finally
        {
            if (shouldClose)
                await connection.CloseAsync();
        }
    }
}
