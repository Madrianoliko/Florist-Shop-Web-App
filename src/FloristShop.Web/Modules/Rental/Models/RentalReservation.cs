using FloristShop.Web.Infrastructure.Data;

namespace FloristShop.Web.Modules.Rental.Models;

/// <summary>
/// Rezerwacja wypożyczenia rekwizytów — zakres dat + lista pozycji + dane kontaktowe klienta.
///
/// Klient zamawia jako GOŚĆ (zgodnie z decyzją MVP), więc dane kontaktowe siedzą wprost
/// na encji — bez osobnej tabeli Customer. Jeśli kiedyś dorzucimy konta klientów,
/// dodamy opcjonalne pole CustomerId (FK) i zachowamy wbudowane dane jako historyczne.
/// </summary>
public class RentalReservation : EntityBase
{
    /// <summary>Numer rezerwacji widoczny dla klienta, format "KNK-R-2026-00001".</summary>
    public string ReservationNumber { get; set; } = string.Empty;

    /// <summary>Data odbioru rekwizytów. DateOnly (nie DateTime) — bo wypożyczenie idzie w dniach.</summary>
    public DateOnly StartDate { get; set; }

    /// <summary>Data zwrotu rekwizytów.</summary>
    public DateOnly EndDate { get; set; }

    public RentalReservationStatus Status { get; set; } = RentalReservationStatus.Pending;

    // === Dane kontaktowe klienta (embedded, bez encji Customer) ===
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;

    /// <summary>Notatka od klienta (np. "potrzebuję dostawy" lub specjalne wymagania).</summary>
    public string? CustomerNote { get; set; }

    /// <summary>Notatka wewnętrzna admina (niewidoczna dla klienta).</summary>
    public string? AdminNote { get; set; }

    /// <summary>Suma cen pozycji — snapshot w momencie potwierdzenia rezerwacji.</summary>
    public decimal TotalPrice { get; set; }

    // === Relacja ===
    public List<RentalReservationItem> Items { get; set; } = new();
}
