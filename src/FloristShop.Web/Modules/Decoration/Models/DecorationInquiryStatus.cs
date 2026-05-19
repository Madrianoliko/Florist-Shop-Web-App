namespace FloristShop.Web.Modules.Decoration.Models;

/// <summary>
/// Workflow zapytania o usługę dekoracji:
/// New -> InQuoting -> QuoteSent -> Accepted -> Completed (happy path)
/// Z każdego etapu można pójść w Rejected (klient odrzuca wycenę) lub Cancelled (cokolwiek innego).
/// </summary>
public enum DecorationInquiryStatus
{
    New = 0,        // Nowe zapytanie od klienta
    InQuoting,      // Admin pracuje nad wyceną
    QuoteSent,      // Wycena wysłana do klienta, czekamy na decyzję
    Accepted,       // Klient zaakceptował, realizacja w toku
    Rejected,       // Klient odrzucił wycenę
    Completed,      // Wykonane
    Cancelled       // Anulowane (z innego powodu)
}
