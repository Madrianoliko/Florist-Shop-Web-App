using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Dictionary.Models;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Dictionary.Services;

/// <summary>
/// Serwis dostępu do słowników. Używany przez formularze edycji w adminie (dropdown kategorii)
/// i przez strony publiczne (wyświetlanie etykiet).
/// </summary>
public class DictionaryService(AppDbContext db)
{
    public static class Types
    {
        public const string RentalCategory  = "rental_category";
        public const string BouquetCategory = "bouquet_category";
        public const string GalleryCategory = "gallery_category";
    }

    /// <summary>Zwraca aktywne wpisy danego słownika, posortowane po SortOrder.</summary>
    public async Task<List<DictionaryEntry>> GetActiveAsync(string type, CancellationToken ct = default)
    {
        return await db.Set<DictionaryEntry>()
            .AsNoTracking()
            .Where(e => e.Type == type && e.IsActive)
            .OrderBy(e => e.SortOrder)
            .ThenBy(e => e.Label)
            .ToListAsync(ct);
    }

    /// <summary>Zwraca wszystkie wpisy (aktywne i nieaktywne) — do zarządzania w adminie.</summary>
    public async Task<List<DictionaryEntry>> GetAllAsync(string type, CancellationToken ct = default)
    {
        return await db.Set<DictionaryEntry>()
            .AsNoTracking()
            .Where(e => e.Type == type)
            .OrderBy(e => e.SortOrder)
            .ThenBy(e => e.Label)
            .ToListAsync(ct);
    }

    /// <summary>Słownik Key → Label dla szybkiego wyświetlania etykiet bez dodatkowych zapytań.</summary>
    public async Task<Dictionary<string, string>> GetLabelMapAsync(string type, CancellationToken ct = default)
    {
        var entries = await GetActiveAsync(type, ct);
        return entries.ToDictionary(e => e.Key, e => e.Label);
    }

    public async Task CreateAsync(string type, string key, string label, int sortOrder, CancellationToken ct = default)
    {
        var entry = new DictionaryEntry
        {
            Type      = type.Trim(),
            Key       = key.Trim(),
            Label     = label.Trim(),
            SortOrder = sortOrder,
            IsActive  = true
        };
        db.Set<DictionaryEntry>().Add(entry);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(int id, string label, int sortOrder, bool isActive, CancellationToken ct = default)
    {
        var entry = await db.Set<DictionaryEntry>().FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entry is null) return;
        entry.Label     = label.Trim();
        entry.SortOrder = sortOrder;
        entry.IsActive  = isActive;
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entry = await db.Set<DictionaryEntry>().FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entry is null) return;
        db.Set<DictionaryEntry>().Remove(entry);
        await db.SaveChangesAsync(ct);
    }
}
