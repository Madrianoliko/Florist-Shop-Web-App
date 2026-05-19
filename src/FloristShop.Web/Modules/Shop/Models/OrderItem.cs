using FloristShop.Web.Infrastructure.Data;

namespace FloristShop.Web.Modules.Shop.Models;

/// <summary>
/// Pozycja zamówienia — odwołanie do BouquetTemplate + ilość + cena/nazwa snapshot.
///
/// Snapshot ceny i nazwy: jeśli kiedyś usuniecie szablon "Pastelowy zachwyt" z katalogu
/// lub zmienicie jego nazwę/cenę, historyczne zamówienia dalej będą wyświetlać poprawne dane.
/// </summary>
public class OrderItem : EntityBase
{
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }

    public Guid BouquetTemplateId { get; set; }
    public BouquetTemplate? BouquetTemplate { get; set; }

    /// <summary>Nazwa bukietu w momencie zamówienia (snapshot).</summary>
    public string BouquetNameSnapshot { get; set; } = string.Empty;

    public int Quantity { get; set; }

    /// <summary>Cena za sztukę w momencie zamówienia (snapshot).</summary>
    public decimal UnitPriceSnapshot { get; set; }
}
