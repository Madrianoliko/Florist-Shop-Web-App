using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Shop.Models;
using FloristShop.Web.Modules.Shop.Services;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Shop;

/// <summary>
/// Rejestracja serwisów modułu Shop.
/// Wywoływana w Program.cs jako builder.Services.AddShopModule().
/// </summary>
public static class ShopModuleExtensions
{
    public static IServiceCollection AddShopModule(this IServiceCollection services)
    {
        services.AddScoped<BouquetCatalogService>();
        services.AddScoped<OrderService>();

        return services;
    }

    /// <summary>
    /// Wstawia przykładowe szablony bukietów w trybie Development.
    /// Idempotentne — jeśli BouquetTemplate już istnieją, nie robi nic.
    /// </summary>
    public static async Task SeedShopDevelopmentDataAsync(AppDbContext db)
    {
        if (await db.Set<BouquetTemplate>().AnyAsync())
        {
            return;
        }

        var templates = new List<BouquetTemplate>
        {
            new()
            {
                Name = "Pastelowy zachwyt",
                Description = "Delikatny bukiet w pastelowych różach, eustomach i gipsówce. " +
                              "Idealna propozycja na urodziny lub jako wyraz uczuć.",
                BasePrice = 120.00m,
                Category = "Birthday",
                ImagePaths = [],
                IsActive = true
            },
            new()
            {
                Name = "Biała klasyka ślubna",
                Description = "Elegancki bukiet ślubny — białe piwonie, liście eukaliptusa i delikatne " +
                              "białe goździki. Ponadczasowy wybór dla Panny Młodej.",
                BasePrice = 280.00m,
                Category = "Wedding",
                ImagePaths = [],
                IsActive = true
            },
            new()
            {
                Name = "Czerwone serce",
                Description = "Klasyczne czerwone róże — symbol miłości i namiętności. " +
                              "Idealny na walentynki, rocznicę lub wyznanie miłości.",
                BasePrice = 150.00m,
                Category = "Romantic",
                ImagePaths = [],
                IsActive = true
            },
            new()
            {
                Name = "Spokój i pamięć",
                Description = "Stonowany bukiet kondolencyjny z białych lilii, chryzantem i zieleni. " +
                              "Wyraża szacunek i pamięć o bliskich.",
                BasePrice = 130.00m,
                Category = "Sympathy",
                ImagePaths = [],
                IsActive = true
            },
            new()
            {
                Name = "Kolorowa radość",
                Description = "Żywy mix sezonowych kwiatów w ciepłych barwach — słoneczniki, gerbery, " +
                              "pomarańczowe tulipany. Idealny na każdą okazję.",
                BasePrice = 90.00m,
                Category = "Everyday",
                ImagePaths = [],
                IsActive = true
            },
            new()
            {
                Name = "Złota rocznica",
                Description = "Żółte i złote kwiaty — nagietek, złote słoneczniki, żółte frezie. " +
                              "Piękny prezent na rocznicę lub jubileusz.",
                BasePrice = 160.00m,
                Category = "Anniversary",
                ImagePaths = [],
                IsActive = true
            }
        };

        db.Set<BouquetTemplate>().AddRange(templates);
        await db.SaveChangesAsync();
    }
}
