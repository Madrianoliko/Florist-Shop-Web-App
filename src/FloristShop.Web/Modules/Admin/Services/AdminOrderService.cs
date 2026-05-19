using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Modules.Shop.Models;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Modules.Admin.Services;

/// <summary>
/// Serwis admina dla modułu Shop — CRUD dla zamówień.
/// Analogiczny do AdminRentalService i AdminDecorationService.
/// </summary>
public class AdminOrderService(AppDbContext db)
{
    public async Task<List<Order>> GetAllOrdersAsync(CancellationToken ct = default)
    {
        return await db.Set<Order>()
            .AsNoTracking()
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Set<Order>()
            .AsNoTracking()
            .Include(o => o.Items)
            .ThenInclude(i => i.BouquetTemplate)
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }

    public async Task UpdateOrderAsync(
        Guid id,
        OrderStatus status,
        PaymentStatus paymentStatus,
        string? adminNote,
        CancellationToken ct = default)
    {
        var order = await db.Set<Order>().FirstOrDefaultAsync(o => o.Id == id, ct);
        if (order is null) return;

        order.Status = status;
        order.PaymentStatus = paymentStatus;
        order.AdminNote = string.IsNullOrWhiteSpace(adminNote) ? null : adminNote.Trim();

        await db.SaveChangesAsync(ct);
    }
}
