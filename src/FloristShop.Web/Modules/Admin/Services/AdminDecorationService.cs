using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Decoration.Models;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Admin.Services;

/// <summary>
/// Serwis admina dla modułu dekoracji.
/// Operacje read + write na zapytaniach ofertowych.
/// </summary>
public class AdminDecorationService(AppDbContext db)
{
    public async Task<List<DecorationInquiry>> GetAllInquiriesAsync(CancellationToken ct = default)
    {
        return await db.Set<DecorationInquiry>()
            .AsNoTracking()
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<DecorationInquiry?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Set<DecorationInquiry>()
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id, ct);
    }

    public async Task UpdateInquiryAsync(
        Guid id,
        DecorationInquiryStatus newStatus,
        string? adminNote,
        decimal? finalPrice,
        CancellationToken ct = default)
    {
        var inquiry = await db.Set<DecorationInquiry>()
            .FirstOrDefaultAsync(i => i.Id == id, ct);

        if (inquiry is null) return;

        inquiry.Status = newStatus;
        if (adminNote is not null) inquiry.AdminNote = adminNote;
        if (finalPrice.HasValue) inquiry.FinalPrice = finalPrice;

        await db.SaveChangesAsync(ct);
    }
}
