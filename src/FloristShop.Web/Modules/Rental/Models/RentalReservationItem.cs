using FloristShop.Web.Infrastructure.Data;

namespace FloristShop.Web.Modules.Rental.Models;

/// <summary>
/// Pozycja w rezerwacji — łączy RentalReservation z konkretnym RentalItem
/// + ilość + cena snapshot.
///
/// Snapshot ceny: w momencie potwierdzenia rezerwacji zapisujemy aktualną cenę.
/// Gdy cennik się zmieni — historyczne rezerwacje pozostają stabilne.
/// </summary>
public class RentalReservationItem : EntityBase
{
    public Guid RentalReservationId { get; set; }
    public RentalReservation? RentalReservation { get; set; }

    public Guid RentalItemId { get; set; }
    public RentalItem? RentalItem { get; set; }

    public int Quantity { get; set; }

    /// <summary>Cena za sztukę w momencie rezerwacji (snapshot).</summary>
    public decimal UnitPriceSnapshot { get; set; }
}
