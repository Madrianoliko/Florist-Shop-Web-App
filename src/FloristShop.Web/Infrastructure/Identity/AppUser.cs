using Microsoft.AspNetCore.Identity;

namespace FloristShop.Web.Infrastructure.Identity;

/// <summary>
/// Użytkownik systemu. W MVP — wyłącznie administratorzy
/// (brat, bratowa, Adrian). Klienci końcowi zamawiają jako goście, bez konta.
///
/// Rozszerzamy IdentityUser o jedno dodatkowe pole — FullName.
/// Dzięki temu w panelu admina możemy wyświetlać "Jan Kowalski"
/// zamiast adresu email, bez dodatkowych zapytań i mapowań.
///
/// === Dlaczego IdentityUser (string TKey), nie IdentityUser&lt;Guid&gt;? ===
/// Identity natywnie używa string jako klucza (przechowuje Guid jako tekst).
/// Przerzucenie na IdentityUser&lt;Guid&gt; wymaga konsekwentnej zmiany typów wszędzie:
/// IdentityRole&lt;Guid&gt;, IdentityDbContext&lt;AppUser, IdentityRole&lt;Guid&gt;, Guid&gt;,
/// AddIdentity z typowanym TKey itd. Dla MVP — niewarte ceremonii.
/// Granica między domeną (Guid) a Identity (string) jest naturalna,
/// nigdzie się nie przenika.
/// </summary>
public class AppUser : IdentityUser
{
    /// <summary>Pełne imię i nazwisko — do wyświetlania w UI panelu admina.</summary>
    public string? FullName { get; set; }
}
