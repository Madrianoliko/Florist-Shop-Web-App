using FloristShop.Web.Infrastructure.Data;

namespace FloristShop.Web.Modules.Gallery.Models;

/// <summary>
/// Zdjęcie z realizacji — portfolio kwiaciarni.
/// Wyświetlane publicznie na stronie /realizacje.
///
/// Category: Key ze słownika "gallery_category" (np. "Wedding", "Church").
/// DisplayOrder: niższy = wyżej na stronie. Admin ustawia ręcznie.
/// </summary>
public class GalleryPhoto : EntityBase
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string ImagePath { get; set; } = string.Empty;

    /// <summary>Key z dictionary_entries (type = "gallery_category").</summary>
    public string Category { get; set; } = "Other";

    /// <summary>Kolejność wyświetlania. Niższy = wcześniej. Domyślnie 0.</summary>
    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;
}
