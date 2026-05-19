namespace FloristShop.Web.Modules.Shop.Models;

public enum DeliveryMethod
{
    Pickup = 0,         // Klient odbiera w kwiaciarni
    LocalDelivery,      // Dostawa lokalna przez kwiaciarnię
    Courier             // Wysyłka kurierem (poza obszar lokalnej dostawy)
}
