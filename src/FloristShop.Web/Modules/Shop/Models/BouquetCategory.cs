namespace FloristShop.Web.Modules.Shop.Models;

/// <summary>
/// Kategorie bukietów w katalogu sklepu.
/// Lista rośnie świadomie (wymaga deploya). Jeśli kiedyś trzeba będzie zarządzać kategoriami
/// z panelu admina — zamienimy na osobną tabelę.
/// </summary>
public enum BouquetCategory
{
    Other = 0,
    Wedding,        // Bukiety ślubne
    Anniversary,    // Rocznicowe
    Birthday,       // Urodzinowe
    Sympathy,       // Kondolencyjne / żałobne
    Romantic,       // Romantyczne (np. walentynki)
    Everyday        // Codzienne / "z okazji niczego"
}
