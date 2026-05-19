namespace FloristShop.Web.Modules.Decoration.Models;

/// <summary>
/// Typy wydarzeń, na które kwiaciarnia świadczy usługi dekoracji.
/// </summary>
public enum DecorationEventType
{
    Other = 0,
    Wedding,        // Ślub / wesele
    Church,         // Kościół (komunia, chrzciny, inne kościelne)
    Anniversary,    // Rocznica
    Funeral         // Pogrzeb
}
