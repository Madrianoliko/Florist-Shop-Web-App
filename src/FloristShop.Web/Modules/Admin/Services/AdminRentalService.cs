using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Rental.Models;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Admin.Services;

/// <summary>
/// Serwis admina dla modułu wypożyczalni.
/// Operacje read + write na rezerwacjach — tylko dla zalogowanych adminów.
///
/// Oddzielony od RentalReservationService (który służy klientom), żeby nie
/// mieszać "co klient może zrobić" z "co admin może zrobić".
/// Przy większym projekcie granica staje się ważna — tutaj to też dobry nawyk.
/// </summary>
public class AdminRentalService(AppDbContext db)
{
    /// <summary>
    /// Zwraca wszystkie rezerwacje posortowane od najnowszej.
    /// AsNoTracking — czytamy tylko do wyświetlenia listy, EF nie musi śledzić zmian.
    /// </summary>
    public async Task<List<RentalReservation>> GetAllReservationsAsync(CancellationToken ct = default)
    {
        return await db.Set<RentalReservation>()
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Zwraca pojedynczą rezerwację ze szczegółami pozycji (Items + powiązany RentalItem).
    /// Null jeśli nie istnieje — strona admina powinna obsłużyć ten przypadek (404 lub redirect).
    /// </summary>
    public async Task<RentalReservation?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Set<RentalReservation>()
            .AsNoTracking()
            .Include(r => r.Items)
                .ThenInclude(i => i.RentalItem)
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    /// <summary>
    /// Aktualizuje status rezerwacji i (opcjonalnie) notatkę admina.
    ///
    /// AdminNote = null → nie nadpisujemy istniejącej notatki.
    /// AdminNote = "" → czyścimy notatkę.
    /// AdminNote = "tekst" → ustawiamy nową treść.
    ///
    /// Wzorzec: najpierw fetch + modify + save (nie ExecuteUpdate), bo chcemy
    /// żeby UpdatedAt w EntityBase.SaveChanges się ustawił automatycznie.
    /// </summary>
    public async Task UpdateReservationAsync(
        Guid id,
        RentalReservationStatus newStatus,
        string? adminNote,
        CancellationToken ct = default)
    {
        var reservation = await db.Set<RentalReservation>()
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (reservation is null)
            return; // Rezerwacja usunięta między GET a POST — ignorujemy cicho

        reservation.Status = newStatus;

        // Nadpisujemy notatkę tylko jeśli admin podał wartość (nawet pustą string)
        if (adminNote is not null)
            reservation.AdminNote = adminNote;

        await db.SaveChangesAsync(ct);
    }
}
