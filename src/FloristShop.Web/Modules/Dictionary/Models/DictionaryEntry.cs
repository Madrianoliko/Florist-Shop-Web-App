namespace FloristShop.Web.Modules.Dictionary.Models;

/// <summary>
/// Wpis słownikowy — generyczny mechanizm zarządzania listami wyboru bez deployowania kodu.
///
/// Type: identyfikator słownika, np. "rental_category", "bouquet_category", "gallery_category".
/// Key:  stabilny klucz przechowywany w encjach (np. RentalItem.Category = "Vase").
///       NIE ZMIENIAJ Key istniejących wpisów — unieważniłbyś dane w tabelach.
/// Label: wyświetlana nazwa (polska), można swobodnie zmieniać.
///
/// Wzorzec: RentalItem.Category (string) przechowuje Key. Label pobierany przez DictionaryService.
/// </summary>
public class DictionaryEntry
{
    public int Id { get; set; }

    /// <summary>"rental_category" | "bouquet_category" | "gallery_category"</summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>Stabilny klucz — przechowywany w encjach domenowych. Nie zmieniaj po wstawieniu.</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Nazwa wyświetlana w UI — można zmieniać, nie wpływa na dane domenowe.</summary>
    public string Label { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;
}
