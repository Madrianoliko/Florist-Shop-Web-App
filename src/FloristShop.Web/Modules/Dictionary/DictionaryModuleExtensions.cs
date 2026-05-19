using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Dictionary.Models;
using FloristShop.Web.Modules.Dictionary.Services;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Dictionary;

public static class DictionaryModuleExtensions
{
    public static IServiceCollection AddDictionaryModule(this IServiceCollection services)
    {
        services.AddScoped<DictionaryService>();
        return services;
    }

    /// <summary>
    /// Seed słowników — wstawia wbudowane kategorie jeśli tabela jest pusta.
    /// Idempotentne.
    /// </summary>
    public static async Task SeedDictionaryDataAsync(AppDbContext db)
    {
        if (await db.Set<DictionaryEntry>().AnyAsync()) return;

        var entries = new List<DictionaryEntry>
        {
            // === Kategorie rekwizytów ===
            new() { Type = DictionaryService.Types.RentalCategory,  Key = "Vase",         Label = "Wazon",              SortOrder = 0 },
            new() { Type = DictionaryService.Types.RentalCategory,  Key = "CandleHolder",  Label = "Świecznik",          SortOrder = 1 },
            new() { Type = DictionaryService.Types.RentalCategory,  Key = "Arch",          Label = "Łuk / brama",        SortOrder = 2 },
            new() { Type = DictionaryService.Types.RentalCategory,  Key = "Lantern",       Label = "Lampion",            SortOrder = 3 },
            new() { Type = DictionaryService.Types.RentalCategory,  Key = "TableDecor",    Label = "Dekoracja stołu",    SortOrder = 4 },
            new() { Type = DictionaryService.Types.RentalCategory,  Key = "Other",         Label = "Inne",               SortOrder = 99 },

            // === Kategorie bukietów ===
            new() { Type = DictionaryService.Types.BouquetCategory, Key = "Wedding",       Label = "Ślubne",             SortOrder = 0 },
            new() { Type = DictionaryService.Types.BouquetCategory, Key = "Anniversary",   Label = "Rocznicowe",         SortOrder = 1 },
            new() { Type = DictionaryService.Types.BouquetCategory, Key = "Birthday",      Label = "Urodzinowe",         SortOrder = 2 },
            new() { Type = DictionaryService.Types.BouquetCategory, Key = "Sympathy",      Label = "Kondolencyjne",      SortOrder = 3 },
            new() { Type = DictionaryService.Types.BouquetCategory, Key = "Romantic",      Label = "Romantyczne",        SortOrder = 4 },
            new() { Type = DictionaryService.Types.BouquetCategory, Key = "Everyday",      Label = "Na co dzień",        SortOrder = 5 },
            new() { Type = DictionaryService.Types.BouquetCategory, Key = "Other",         Label = "Inne",               SortOrder = 99 },

            // === Kategorie galerii ===
            new() { Type = DictionaryService.Types.GalleryCategory, Key = "Wedding",       Label = "Ślub / wesele",      SortOrder = 0 },
            new() { Type = DictionaryService.Types.GalleryCategory, Key = "Church",        Label = "Uroczystość kościelna", SortOrder = 1 },
            new() { Type = DictionaryService.Types.GalleryCategory, Key = "Anniversary",   Label = "Rocznica / jubileusz", SortOrder = 2 },
            new() { Type = DictionaryService.Types.GalleryCategory, Key = "Birthday",      Label = "Urodziny",           SortOrder = 3 },
            new() { Type = DictionaryService.Types.GalleryCategory, Key = "Other",         Label = "Inne",               SortOrder = 99 },
        };

        db.Set<DictionaryEntry>().AddRange(entries);
        await db.SaveChangesAsync();
    }
}
