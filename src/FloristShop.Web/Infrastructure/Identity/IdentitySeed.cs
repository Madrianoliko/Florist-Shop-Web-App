using Microsoft.AspNetCore.Identity;

namespace FloristShop.Web.Infrastructure.Identity;

/// <summary>
/// Seed roli "Admin" + administratorów z appsettings.{env}.json.
///
/// Idempotentny — wywołanie wielokrotne nie tworzy duplikatów:
/// - Rola "Admin" tworzona tylko gdy nie istnieje.
/// - Każdy user tworzony tylko gdy email nie jest jeszcze w bazie.
///
/// Wywoływany w Development z Program.cs po zaaplikowaniu migracji.
/// </summary>
public static class IdentitySeed
{
    /// <summary>Nazwa jedynej roli systemu w MVP.</summary>
    public const string AdminRole = "Admin";

    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();

        // 1. Rola Admin
        if (!await roleManager.RoleExistsAsync(AdminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(AdminRole));
        }

        // 2. Lista adminów z konfiguracji
        var admins = configuration
            .GetSection("AdminUsers")
            .Get<List<AdminUserDefinition>>() ?? new List<AdminUserDefinition>();

        // 3. Tworzymy każdego (jeśli nie istnieje)
        foreach (var def in admins)
        {
            // Pomijamy puste wpisy
            if (string.IsNullOrWhiteSpace(def.Email) || string.IsNullOrWhiteSpace(def.Password))
            {
                continue;
            }

            // Jeśli admin o tym emailu już istnieje — przeskakujemy
            var existing = await userManager.FindByEmailAsync(def.Email);
            if (existing is not null)
            {
                continue;
            }

            var user = new AppUser
            {
                UserName = def.Email,
                Email = def.Email,
                FullName = def.FullName,
                EmailConfirmed = true   // seedowanego admina nie wysyłamy na weryfikację maila
            };

            var createResult = await userManager.CreateAsync(user, def.Password);
            if (!createResult.Succeeded)
            {
                // Fail-fast z czytelnym komunikatem — często to niezgodność haseł z PasswordOptions
                throw new InvalidOperationException(
                    $"Nie udało się utworzyć admin user '{def.Email}': " +
                    string.Join("; ", createResult.Errors.Select(e => $"{e.Code}: {e.Description}")));
            }

            await userManager.AddToRoleAsync(user, AdminRole);
        }
    }
}
