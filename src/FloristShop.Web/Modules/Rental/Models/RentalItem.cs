using FloristShop.Web.Infrastructure.Data;

namespace FloristShop.Web.Modules.Rental.Models;

/// <summary>
/// Rekwizyt w katalogu wypożyczalni.
/// Reprezentuje TYP przedmiotu (np. "Wazon cylindryczny szklany 40cm"),
/// a nie pojedynczy egzemplarz. Pole TotalQuantity określa ile fizycznych sztuk macie w magazynie.
///
/// Decyzja modelowa: NIE śledzimy pojedynczych egzemplarzy (jak RentalItemInstance).
/// W MVP wystarczy nam "ile sztuk wazonu jest wolnych w danym dniu" — to wyliczamy z rezerwacji.
/// Jeśli kiedyś będzie potrzeba ewidencji per sztuka (uszkodzenia, historia konkretnego egzemplarza),
/// dorzucimy encję RentalItemInstance i FK do RentalItem.
/// </summary>
public class RentalItem : EntityBase
{
    /// <summary>Nazwa katalogowa, np. "Wazon cylindryczny szklany 40cm".</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Opcjonalny dłuższy opis (materiał, wymiary, kontekst stylistyczny).</summary>
    public string? Description { get; set; }

    /// <summary>Klucz z dictionary_entries (type = "rental_category"), np. "Vase", "Arch".</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>Cena za jeden okres wypożyczenia (PLN). W MVP bez stawek dziennych.</summary>
    public decimal PricePerRental { get; set; }

    /// <summary>Liczba sztuk fizycznie posiadanych w magazynie.</summary>
    public int TotalQuantity { get; set; }

    /// <summary>
    /// Ścieżki do zdjęć (relatywne, np. "uploads/rental/abc.jpg").
    /// W bazie trzymane jako JSONB — pole nie wymaga osobnej tabeli image_paths.
    /// Npgsql provider sam serializuje List&lt;string&gt; do JSONB.
    /// </summary>
    public List<string> ImagePaths { get; set; } = new();

    /// <summary>
    /// Czy aktualnie pokazujemy w katalogu. false = ukryte
    /// (np. uszkodzone, sezonowo wycofane, czeka na naprawę).
    /// </summary>
    public bool IsActive { get; set; } = true;
}
