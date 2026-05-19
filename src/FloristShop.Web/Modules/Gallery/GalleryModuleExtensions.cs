using FloristShop.Web.Modules.Gallery.Services;

namespace FloristShop.Web.Modules.Gallery;

public static class GalleryModuleExtensions
{
    public static IServiceCollection AddGalleryModule(this IServiceCollection services)
    {
        services.AddScoped<GalleryService>();
        return services;
    }
}
