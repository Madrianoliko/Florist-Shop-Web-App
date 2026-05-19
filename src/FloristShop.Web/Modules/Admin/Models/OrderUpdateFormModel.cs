using FloristShop.Web.Modules.Shop.Models;

namespace FloristShop.Web.Modules.Admin.Models;

/// <summary>
/// Model formularza aktualizacji zamówienia w panelu admina.
/// Oddzielny plik — klasy nie można definiować poza blokiem @code w .razor.
/// </summary>
public class OrderUpdateFormModel
{
    public OrderStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string? AdminNote { get; set; }
}
