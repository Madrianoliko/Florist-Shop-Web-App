using FloristShop.Web.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FloristShop.Web.Infrastructure.Data;

/// <summary>
/// Centralny DbContext aplikacji. Wspólny dla wszystkich modułów (modular monolith).
///
/// Dziedziczymy z IdentityDbContext&lt;AppUser&gt; — to dorzuca 7 tabel Identity
/// (AspNetUsers, AspNetRoles, AspNetUserRoles, AspNetUserClaims, AspNetRoleClaims,
/// AspNetUserLogins, AspNetUserTokens). Trzymamy je w tej samej bazie i tym samym
/// DbContextcie co domenę — solo dev, jedna baza, brak korzyści z separacji.
///
/// Świadomie NIE deklarujemy tutaj DbSet&lt;T&gt; dla każdej naszej encji domenowej.
/// Zamiast tego każdy moduł trzyma swoją encję i jej IEntityTypeConfiguration u siebie,
/// a DbContext wczytuje wszystkie konfiguracje automatycznie przez ApplyConfigurationsFromAssembly.
/// Dzięki temu dodanie nowej encji NIE wymaga modyfikacji AppDbContext — wystarczy stworzyć
/// klasę encji + jej konfigurację w swoim module.
///
/// Z encjami pracujemy przez Set&lt;T&gt;() albo przez wstrzykiwany w serwisach kontekst.
/// </summary>
public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // WAŻNE: base.OnModelCreating MUSI iść PIERWSZE.
        // IdentityDbContext w nim konfiguruje tabele Identity (AspNetUsers itd.).
        // Nasze ApplyConfigurationsFromAssembly idzie potem — jeśli kiedyś chcielibyśmy
        // nadpisać coś z konfiguracji Identity (np. zmienić nazwy kolumn), nasza
        // konfiguracja przeleciałaby na końcu i wygrała.
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    // === Audyt timestampów ===
    // Nadpisujemy SaveChanges/SaveChangesAsync, żeby automatycznie ustawiać UpdatedAt
    // dla każdej modyfikowanej encji dziedziczącej po EntityBase.
    // CreatedAt jest ustawiany w konstruktorze encji (property initializer w EntityBase),
    // więc tu nie musimy się nim zajmować.

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var modifiedEntries = ChangeTracker.Entries<EntityBase>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in modifiedEntries)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
