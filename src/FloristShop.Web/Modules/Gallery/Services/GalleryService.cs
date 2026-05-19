using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Gallery.Models;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Gallery.Services;

/// <summary>
/// Serwis galerii realizacji — używany zarówno przez stronę publiczną jak i admin.
/// </summary>
public class GalleryService(AppDbContext db)
{
    /// <summary>Aktywne zdjęcia — dla strony publicznej.</summary>
    public async Task<List<GalleryPhoto>> GetActiveAsync(CancellationToken ct = default)
    {
        return await db.Set<GalleryPhoto>()
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.DisplayOrder)
            .ThenByDescending(p => p.CreatedAt)
            .ToListAsync(ct);
    }

    /// <summary>Wszystkie zdjęcia — dla admina.</summary>
    public async Task<List<GalleryPhoto>> GetAllAsync(CancellationToken ct = default)
    {
        return await db.Set<GalleryPhoto>()
            .AsNoTracking()
            .OrderBy(p => p.DisplayOrder)
            .ThenByDescending(p => p.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<GalleryPhoto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Set<GalleryPhoto>()
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<Guid> CreateAsync(
        string imagePath,
        string category,
        string? title,
        string? description,
        int displayOrder,
        CancellationToken ct = default)
    {
        var photo = new GalleryPhoto
        {
            ImagePath    = imagePath,
            Category     = category,
            Title        = string.IsNullOrWhiteSpace(title) ? null : title.Trim(),
            Description  = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            DisplayOrder = displayOrder,
            IsActive     = true
        };
        db.Set<GalleryPhoto>().Add(photo);
        await db.SaveChangesAsync(ct);
        return photo.Id;
    }

    public async Task UpdateAsync(
        Guid id,
        string category,
        string? title,
        string? description,
        int displayOrder,
        bool isActive,
        CancellationToken ct = default)
    {
        var photo = await db.Set<GalleryPhoto>().FirstOrDefaultAsync(p => p.Id == id, ct);
        if (photo is null) return;

        photo.Category     = category;
        photo.Title        = string.IsNullOrWhiteSpace(title) ? null : title.Trim();
        photo.Description  = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        photo.DisplayOrder = displayOrder;
        photo.IsActive     = isActive;
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var photo = await db.Set<GalleryPhoto>().FirstOrDefaultAsync(p => p.Id == id, ct);
        if (photo is null) return;
        db.Set<GalleryPhoto>().Remove(photo);
        await db.SaveChangesAsync(ct);
    }
}
