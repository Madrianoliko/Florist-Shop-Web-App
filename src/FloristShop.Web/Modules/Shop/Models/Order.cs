using FloristShop.Web.Infrastructure.Data;

namespace FloristShop.Web.Modules.Shop.Models;

/// <summary>
/// Zamówienie w sklepie z bukietami.
/// Klient zamawia jako gość — wszystkie dane kontaktowe na encji.
/// </summary>
public class Order : EntityBase
{
    /// <summary>Numer zamówienia widoczny dla klienta, format "KNK-O-2026-00001".</summary>
    public string OrderNumber { get; set; } = string.Empty;

    public OrderStatus Status { get; set; } = OrderStatus.New;

    // === Dane kontaktowe ===
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;

    // === Dostawa ===
    public DeliveryMethod DeliveryMethod { get; set; }

    /// <summary>Adres dostawy. Wymagany, gdy DeliveryMethod != Pickup.</summary>
    public string? DeliveryAddress { get; set; }

    /// <summary>Data, na którą klient chce mieć bukiet.</summary>
    public DateOnly DeliveryDate { get; set; }

    // === Notatki ===
    /// <summary>Notatka od klienta — np. "imię na kartce: Anna", "bez liliowych — alergia".</summary>
    public string? CustomerNote { get; set; }

    /// <summary>Notatka wewnętrzna admina.</summary>
    public string? AdminNote { get; set; }

    // === Kwoty ===
    /// <summary>Suma cen pozycji (snapshot).</summary>
    public decimal Subtotal { get; set; }

    /// <summary>Koszt dostawy (0 dla Pickup).</summary>
    public decimal DeliveryFee { get; set; }

    /// <summary>Łączna kwota = Subtotal + DeliveryFee.</summary>
    public decimal TotalPrice { get; set; }

    // === Płatność ===
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;

    // === Pozycje ===
    public List<OrderItem> Items { get; set; } = new();
}
