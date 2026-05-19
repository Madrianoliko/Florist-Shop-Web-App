using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Rental.Models;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Rental.Services;

/// <summary>
/// Serwis katalogu wypożyczalni — operacje read-only na liście rekwizytów.
///
/// === Dlaczego osobny serwis, a nie DbContext bezpośrednio w komponencie? ===
///
/// 1. Komponent UI nie powinien znać szczegółów persystencji (LINQ, .AsNoTracking()).
///    Gdy zmienimy źródło danych (np. dorzucimy cache, zmienimy provider, dodamy
///    fallback do innego storage) — zmiana jest w JEDNYM miejscu, nie w 20 komponentach.
///
/// 2. Logika biznesowa rośnie. "Pokaż katalog" dziś = .Where(IsActive).
///    Za miesiąc może być += "uwzględniaj sezonowość", "ukrywaj wycofane",
///    "filtruj po uprawnieniach". TO NIE NALEŻY do widoku.
///
/// 3. Testowalność. Łatwo testować "serwis zwraca tylko aktywne rekwizyty"
///    (mockujemy DbContext lub używamy InMemory). Trudno testować całą stronę Blazor.
///
/// === Dlaczego BEZ interfejsu (IRentalCatalogService)? ===
///
/// W .NET-owym kanonie często widzisz IService + Service. Pragmatycznie — interfejs
/// dodaje wartość gdy:
/// - Masz wiele implementacji (np. CachedRentalCatalogService + DbRentalCatalogService).
/// - Robisz mocki w testach jednostkowych (choć dla EF Core jest też InMemory provider).
/// - Wystawiasz API publiczne (kontrakty).
///
/// Dla solo dewelopera na MVP — interfejs to zwykle "premature abstraction".
/// Dodamy go w momencie, gdy realnie pojawi się potrzeba.
/// </summary>
public class RentalCatalogService(AppDbContext db)
{
    // Primary constructor (C# 12, .NET 8+) — parametr `db` jest "captured"
    // i dostępny w metodach instancji. Krótszy zapis tradycyjnego DI z private field.

    /// <summary>
    /// Zwraca aktywne pozycje katalogu posortowane po kategorii i nazwie.
    ///
    /// .AsNoTracking() — encje są read-only, EF nie potrzebuje ich śledzić do zmian.
    /// Mniej pamięci, szybsze zapytanie. Standard dla query-only ścieżek.
    ///
    /// IReadOnlyList&lt;T&gt; jako typ zwracany — sygnał dla wołającego "tylko czytaj".
    /// Nie da się przypadkowo dorzucić elementu przez .Add() w UI.
    ///
    /// CancellationToken — dobry nawyk. W Blazor Server gdy klient zerwie połączenie,
    /// można anulować w-locie zapytanie do bazy. Domyślne default = CancellationToken.None.
    /// </summary>
    public async Task<IReadOnlyList<RentalItem>> GetActiveCatalogAsync(CancellationToken ct = default)
    {
        return await db.Set<RentalItem>()
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);
    }
}
