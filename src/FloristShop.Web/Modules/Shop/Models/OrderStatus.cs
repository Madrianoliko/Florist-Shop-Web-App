namespace FloristShop.Web.Modules.Shop.Models;

/// <summary>
/// Workflow zamówienia:
/// New -> Confirmed -> InPreparation -> Ready -> OutForDelivery -> Delivered (happy path)
/// Z każdego etapu (przed Delivered) można pójść w Cancelled.
///
/// "Ready" oznacza: bukiet gotowy do odbioru (pickup) lub do wysyłki.
/// "OutForDelivery" — kurier/dostawa w trasie (jeśli DeliveryMethod != Pickup).
/// </summary>
public enum OrderStatus
{
    New = 0,
    Confirmed,
    InPreparation,
    Ready,
    OutForDelivery,
    Delivered,
    Cancelled
}
