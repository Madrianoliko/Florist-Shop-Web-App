using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Rental.Models;
using FloristShop.Web.Modules.Rental.Services;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Rental;

/// <summary>
/// Wzorzec "moduł deklaruje swoją rejestrację" — każdy moduł domenowy
/// (Rental, Decoration, Shop, Admin) eksponuje JEDNĄ metodę AddXxxModule()
/// wywoływaną w Program.cs.
///
/// Dzięki temu dodanie nowego serwisu / encji / komponentu modułu NIE wymaga
/// zmian w Program.cs — wszystko żyje w obrębie folderu modułu.
/// </summary>
public static class RentalModuleExtensions
{
    /// <summary>
    /// Rejestruje serwisy DI dla modułu Rental.
    ///
    /// Wszystkie serwisy modułu trzymamy jako SCOPED — bo używają DbContext,
    /// który też jest scoped. Lifetime DbContextu i wstrzykiwanego serwisu
    /// MUSI być spójny, w przeciwnym razie ASP.NET Core rzuca "captive dependency"
    /// (np. singleton service trzyma scoped DbContext = ten sam DbContext żyje
    /// przez całe życie aplikacji = wycieki pamięci i tracking-y).
    /// </summary>
    public static IServiceCollection AddRentalModule(this IServiceCollection services)
    {
        services.AddScoped<RentalCatalogService>();
        services.AddScoped<RentalReservationService>();

        return services;
    }

    /// <summary>
    /// Wstawia testowe dane do bazy w trybie Development.
    /// Idempotentne — jeśli RentalItem już istnieją, nie robi nic.
    /// </summary>
    public static async Task SeedRentalDevelopmentDataAsync(AppDbContext db)
    {
        // Sprawdź czy są jakiekolwiek dane — jeśli tak, kończymy bez akcji.
        // AnyAsync() przekłada się na SELECT EXISTS w SQL — szybki query bez ściągania rekordów.
        if (await db.Set<RentalItem>().AnyAsync())
        {
            return;
        }

        var items = new List<RentalItem>
        {
            new()
            {
                Name = "Wazon cylindryczny szklany 40cm",
                Description = "Klasyczny szklany cylinder, idealny do wysokich kompozycji kwiatowych. " +
                              "Przezroczyste szkło sprawdzi się w każdej stylistyce.",
                Category = "Vase",
                PricePerRental = 25.00m,
                TotalQuantity = 20,
                ImagePaths = new List<string>(),
                IsActive = true
            },
            new()
            {
                Name = "Świecznik mosiężny pięcioramienny",
                Description = "Elegancki, vintage. Świetny do dekoracji w stylu boho lub glamour. " +
                              "Pasuje do świec rozmiar 4cm.",
                Category = "CandleHolder",
                PricePerRental = 50.00m,
                TotalQuantity = 8,
                ImagePaths = new List<string>(),
                IsActive = true
            },
            new()
            {
                Name = "Łuk ślubny biały drewniany 2.5m",
                Description = "Demontowalny, łatwy w transporcie. Można przybierać kwiatami, zielenią " +
                              "lub tkaninami. Dwa elementy boczne + belka górna.",
                Category = "Arch",
                PricePerRental = 350.00m,
                TotalQuantity = 2,
                ImagePaths = new List<string>(),
                IsActive = true
            }
        };

        db.Set<RentalItem>().AddRange(items);
        await db.SaveChangesAsync();
    }
}
