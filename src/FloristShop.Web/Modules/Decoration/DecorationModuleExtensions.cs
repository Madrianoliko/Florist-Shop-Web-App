using FloristShop.Web.Modules.Decoration.Services;

namespace FloristShop.Web.Modules.Decoration;

/// <summary>
/// Rejestracja modułu Decoration. Identyczny wzorzec jak RentalModuleExtensions.
/// </summary>
public static class DecorationModuleExtensions
{
    public static IServiceCollection AddDecorationModule(this IServiceCollection services)
    {
        services.AddScoped<DecorationInquiryService>();
        return services;
    }
}
