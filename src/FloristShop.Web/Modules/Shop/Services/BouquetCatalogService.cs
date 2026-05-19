using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Shop.Models;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Shop.Services;

/// <summary>
/// Serwis katalogu bukietów — dostęp tylko do odczytu.
/// Zwraca aktywne szablony (IsActive = true), posortowane alfabetycznie w ramach kategorii.
/// </summary>
public class BouquetCatalogService(AppDbContext db)
{
    public async Task<List<BouquetTemplate>> GetActiveCatalogAsync(CancellationToken ct = default)
    {
        return await db.Set<BouquetTemplate>()
            .AsNoTracking()
            .Where(t => t.IsActive)
            .OrderBy(t => t.Category)
            .ThenBy(t => t.Name)
            .ToListAsync(ct);
    }

    public async Task<BouquetTemplate?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Set<BouquetTemplate>()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, ct);
    }
}
