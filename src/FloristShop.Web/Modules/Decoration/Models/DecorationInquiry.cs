using FloristShop.Web.Infrastructure.Data;

namespace FloristShop.Web.Modules.Decoration.Models;

/// <summary>
/// Zapytanie ofertowe na usługę dekoracji wydarzenia.
///
/// W MVP — pojedyncza encja obsługująca cały lejek: zapytanie -> wycena -> akceptacja -> realizacja.
/// Bez osobnej encji DecorationOffer — wycena lądowała w FinalPrice + AdminNote.
/// Jeśli kiedyś trzeba będzie generować PDF z wyceną, prowadzić więcej iteracji wyceny
/// albo pokazywać klientowi wycenę online — wydzielimy.
/// </summary>
public class DecorationInquiry : EntityBase
{
    /// <summary>Numer zapytania widoczny dla klienta, format "KNK-D-2026-00001".</summary>
    public string InquiryNumber { get; set; } = string.Empty;

    public DecorationEventType EventType { get; set; }

    public DateOnly EventDate { get; set; }

    /// <summary>Miejscowość lub pełny adres miejsca wydarzenia.</summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>Opis wizji klienta: kolory, styl, ilość gości, oczekiwania.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Orientacyjny budżet klienta (opcjonalny — nie wszyscy podają).</summary>
    public decimal? BudgetEstimate { get; set; }

    public DecorationInquiryStatus Status { get; set; } = DecorationInquiryStatus.New;

    // === Dane kontaktowe klienta (embedded) ===
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;

    /// <summary>Notatki wewnętrzne — wyniki rozmów, kalkulacje wyceny, ustalenia.</summary>
    public string? AdminNote { get; set; }

    /// <summary>Ostateczna cena po wycenie (NULL dopóki nie ustalona).</summary>
    public decimal? FinalPrice { get; set; }
}
