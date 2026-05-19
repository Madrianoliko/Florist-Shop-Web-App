namespace FloristShop.Web.Modules.Rental.Models;

/// <summary>
/// Statusy rezerwacji rekwizytów.
/// Workflow: Pending -> Confirmed -> Issued -> Returned (happy path)
/// W każdym momencie do Pending/Confirmed można dorzucić Cancelled.
/// </summary>
public enum RentalReservationStatus
{
    /// <summary>Domyślne — klient zarezerwował, czeka na potwierdzenie admina.</summary>
    Pending = 0,

    /// <summary>Admin potwierdził rezerwację po kontakcie z klientem.</summary>
    Confirmed,

    /// <summary>Rekwizyty wydane klientowi — w trakcie wypożyczenia.</summary>
    Issued,

    /// <summary>Klient zwrócił rekwizyty — rezerwacja zakończona.</summary>
    Returned,

    /// <summary>Anulowana (przez klienta lub admina, na każdym etapie przed Issued).</summary>
    Cancelled
}
