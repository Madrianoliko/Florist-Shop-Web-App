using FloristShop.Web.Modules.Admin.Services;
using FloristShop.Web.Modules.Gallery.Services;

namespace FloristShop.Web.Modules.Admin;

/// <summary>
/// Rejestracja serwisów modułu Admin.
/// Wywoływana w Program.cs jako builder.Services.AddAdminModule().
/// </summary>
public static class AdminModuleExtensions
{
    public static IServiceCollection AddAdminModule(this IServiceCollection services)
    {
        services.AddScoped<AdminRentalService>();
        services.AddScoped<AdminDecorationService>();
        services.AddScoped<AdminOrderService>();
        services.AddScoped<AdminBouquetCatalogService>();
        services.AddScoped<AdminRentalCatalogService>();
        services.AddScoped<GalleryService>();

        return services;
    }
}
