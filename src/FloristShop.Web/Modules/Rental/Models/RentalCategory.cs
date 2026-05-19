namespace FloristShop.Web.Modules.Rental.Models;

/// <summary>
/// Kategorie rekwizytów w wypożyczalni.
///
/// Świadomie używamy ENUMA zamiast osobnej tabeli "rental_categories", bo:
/// - Lista jest mała i rzadko się zmienia (rekwizyty to fizyczne obiekty, nie kategorie produktów).
/// - Brak panelu admina do zarządzania kategoriami na MVP.
/// - Dodanie nowej kategorii wymaga deploya — to akceptowalna cena za prostotę.
///
/// Jeśli kiedyś okaże się, że potrzebujemy edycji kategorii bez deploya
/// (np. brat dzwoni: "dodaj kategorię 'Tła fotograficzne'") — przepiszemy na encję.
/// </summary>
public enum RentalCategory
{
    /// <summary>
    /// Wartość domyślna. Świadomie ustawiona jako 0 — przypadkowo utworzony RentalItem
    /// bez explicit Category wpadnie do "Other", nie do alfabetycznie pierwszej kategorii.
    /// </summary>
    Other = 0,

    Vase,           // Wazony
    CandleHolder,   // Świeczniki
    Arch,           // Łuki (ślubne, dekoracyjne)
    Lantern,        // Lampiony
    TableDecor      // Dekoracje stołu
}
