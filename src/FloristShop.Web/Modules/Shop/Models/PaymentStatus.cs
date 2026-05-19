namespace FloristShop.Web.Modules.Shop.Models;

/// <summary>
/// W MVP płatności są obsługiwane offline (gotówka przy odbiorze, przelew tradycyjny).
/// Admin ręcznie oznacza zamówienie jako Paid po otrzymaniu pieniędzy.
/// Online payments (Stripe, PayU) dorzucimy na końcu projektu — wtedy enum się rozszerzy
/// (np. PendingPayment, FailedPayment, Refunded).
/// </summary>
public enum PaymentStatus
{
    Unpaid = 0,
    Paid
}
