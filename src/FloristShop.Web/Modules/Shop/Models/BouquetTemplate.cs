using FloristShop.Web.Infrastructure.Data;

namespace FloristShop.Web.Modules.Shop.Models;

/// <summary>
/// Szablon bukietu w katalogu sklepu.
///
/// To NIE jest "produkt z magazynu" jak w klasycznym e-commerce.
/// Każdy bukiet robiony jest ręcznie po zamówieniu — szablon to inspiracja
/// (zdjęcie + opis + cena bazowa). Brak pola "stock count" — magazyn dotyczy
/// kwiatów-składników, nie gotowych bukietów.
/// </summary>
public class BouquetTemplate : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    /// <summary>Cena bazowa. Klient może dostosować ostateczną przez customer note.</summary>
    public decimal BasePrice { get; set; }

    /// <summary>Klucz z dictionary_entries (type = "bouquet_category"), np. "Wedding", "Birthday".</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>Ścieżki do zdjęć (JSONB w bazie).</summary>
    public List<string> ImagePaths { get; set; } = new();

    public bool IsActive { get; set; } = true;
}
