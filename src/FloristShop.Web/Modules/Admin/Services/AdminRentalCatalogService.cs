using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Rental.Models;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Admin.Services;

/// <summary>
/// CRUD dla katalogu rekwizytów wypożyczalni — używany przez strony edycji admina.
/// </summary>
public class AdminRentalCatalogService(AppDbContext db)
{
    public async Task<List<RentalItem>> GetAllAsync(CancellationToken ct = default)
    {
        return await db.Set<RentalItem>()
            .AsNoTracking()
            .OrderBy(i => i.Category)
            .ThenBy(i => i.Name)
            .ToListAsync(ct);
    }

    public async Task<RentalItem?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Set<RentalItem>()
            .FirstOrDefaultAsync(i => i.Id == id, ct);
    }

    public async Task<Guid> CreateAsync(
        string name,
        string? description,
        string category,
        decimal pricePerRental,
        int totalQuantity,
        bool isActive,
        List<string> imagePaths,
        CancellationToken ct = default)
    {
        var item = new RentalItem
        {
            Name           = name.Trim(),
            Description    = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            Category       = category,
            PricePerRental = pricePerRental,
            TotalQuantity  = totalQuantity,
            IsActive       = isActive,
            ImagePaths     = imagePaths
        };

        db.Set<RentalItem>().Add(item);
        await db.SaveChangesAsync(ct);
        return item.Id;
    }

    public async Task UpdateAsync(
        Guid id,
        string name,
        string? description,
        string category,
        decimal pricePerRental,
        int totalQuantity,
        bool isActive,
        List<string> imagePaths,
        CancellationToken ct = default)
    {
        var item = await db.Set<RentalItem>()
            .FirstOrDefaultAsync(i => i.Id == id, ct);
        if (item is null) return;

        item.Name           = name.Trim();
        item.Description    = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        item.Category       = category;
        item.PricePerRental = pricePerRental;
        item.TotalQuantity  = totalQuantity;
        item.IsActive       = isActive;
        item.ImagePaths     = imagePaths;

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var item = await db.Set<RentalItem>()
            .FirstOrDefaultAsync(i => i.Id == id, ct);
        if (item is null) return;

        db.Set<RentalItem>().Remove(item);
        await db.SaveChangesAsync(ct);
    }
}
