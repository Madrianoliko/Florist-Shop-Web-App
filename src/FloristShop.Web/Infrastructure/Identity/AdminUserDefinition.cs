namespace FloristShop.Web.Infrastructure.Identity;

/// <summary>
/// Definicja konta admina do seedu — odpowiada wpisom w appsettings.{env}.json
/// w sekcji "AdminUsers". Bind robi standardowy IConfiguration.GetSection().Get<List<T>>().
///
/// Hasła w appsettings.Development.json są lokalnymi danymi dev — NIE commitujemy
/// realnych haseł produkcyjnych. W PROD wstrzyknięmy je przez ENV vars / Azure Key Vault.
/// </summary>
public class AdminUserDefinition
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
