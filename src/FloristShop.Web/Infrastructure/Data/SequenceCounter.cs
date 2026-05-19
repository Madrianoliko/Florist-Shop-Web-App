namespace FloristShop.Web.Infrastructure.Data;

/// <summary>
/// Licznik sekwencji dla numerów trackingowych — KNK-R-2026-00001, KNK-O-2026-00001 itd.
///
/// Klucz główny jest ZŁOŻONY: (Type, Year).
/// Każdy typ dokumentu (R = Rental, O = Order, D = Decoration) ma swoją niezależną
/// sekwencję per rok kalendarzowy — przy nowym roku numeracja startuje od 1.
///
/// Increment i odczyt dzieje się atomowo przez Postgres UPSERT z RETURNING —
/// bez blokad aplikacyjnych, bez race conditions.
/// </summary>
public class SequenceCounter
{
    /// <summary>
    /// Typ dokumentu: "R" = Rental, "O" = Order, "D" = Decoration.
    /// Jeden znak wystarczy, ale zostawiamy miejsce na rozszerzenie.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>Rok kalendarzowy — sekwencja resetuje się co rok.</summary>
    public int Year { get; set; }

    /// <summary>
    /// Ostatnio użyta wartość. Następny numer = LastValue + 1.
    /// Wartość domyślna to 0 — pierwszy dokument dostaje 1.
    /// </summary>
    public int LastValue { get; set; }
}
