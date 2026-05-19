using FloristShop.Web.Modules.Rental.Models;

namespace FloristShop.Web.Modules.Admin.Models;

/// <summary>
/// Model formularza aktualizacji rezerwacji przez admina.
/// Tylko pola edytowalne — nie całe RentalReservation.
/// </summary>
public class ReservationUpdateFormModel
{
    public RentalReservationStatus Status { get; set; }
    public string? AdminNote { get; set; }
}
