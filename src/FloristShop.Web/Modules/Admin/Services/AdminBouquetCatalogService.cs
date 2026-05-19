using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Shop.Models;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Admin.Services;

/// <summary>
/// CRUD dla katalogu bukietów — używany przez strony edycji admina.
/// </summary>
public class AdminBouquetCatalogService(AppDbContext db)
{
    public async Task<List<BouquetTemplate>> GetAllAsync(CancellationToken ct = default)
    {
        return await db.Set<BouquetTemplate>()
            .AsNoTracking()
            .OrderBy(t => t.Category)
            .ThenBy(t => t.Name)
            .ToListAsync(ct);
    }

    public async Task<BouquetTemplate?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Set<BouquetTemplate>()
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<Guid> CreateAsync(
        string name,
        string? description,
        decimal basePrice,
        string category,
        bool isActive,
        List<string> imagePaths,
        CancellationToken ct = default)
    {
        var template = new BouquetTemplate
        {
            Name        = name.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            BasePrice   = basePrice,
            Category    = category,
            IsActive    = isActive,
            ImagePaths  = imagePaths
        };

        db.Set<BouquetTemplate>().Add(template);
        await db.SaveChangesAsync(ct);
        return template.Id;
    }

    public async Task UpdateAsync(
        Guid id,
        string name,
        string? description,
        decimal basePrice,
        string category,
        bool isActive,
        List<string> imagePaths,
        CancellationToken ct = default)
    {
        var template = await db.Set<BouquetTemplate>()
            .FirstOrDefaultAsync(t => t.Id == id, ct);
        if (template is null) return;

        template.Name        = name.Trim();
        template.Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        template.BasePrice   = basePrice;
        template.Category    = category;
        template.IsActive    = isActive;
        template.ImagePaths  = imagePaths;

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var template = await db.Set<BouquetTemplate>()
            .FirstOrDefaultAsync(t => t.Id == id, ct);
        if (template is null) return;

        db.Set<BouquetTemplate>().Remove(template);
        await db.SaveChangesAsync(ct);
    }
}
