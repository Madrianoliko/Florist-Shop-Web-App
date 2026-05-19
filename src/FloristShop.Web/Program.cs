using FloristShop.Web.Components;
using FloristShop.Web.Infrastructure;
using FloristShop.Web.Infrastructure.Data;
using FloristShop.Web.Infrastructure.Identity;
using FloristShop.Web.Modules.Admin;
using FloristShop.Web.Modules.Decoration;
using FloristShop.Web.Modules.Dictionary;
using FloristShop.Web.Modules.Gallery;
using FloristShop.Web.Modules.Rental;
using FloristShop.Web.Modules.Shop;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// === Port — Railway sets PORT dynamically ===
// Locally: default 5000/5001. On Railway: whatever Railway assigns.
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Increase SignalR message size limit — default 32 KB blocks upload of
// cropped images (base64 of a large image can be several hundred KB).
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10 MB
});

builder.Services.AddCascadingAuthenticationState();

// === Database ===
// Priority order (first non-empty wins):
// 1. DATABASE_URL  — Railway: paste as plain value in web service Variables
// 2. PGHOST + other PG* vars — Railway also exports these as fallback
// 3. ConnectionStrings:DefaultConnection — local appsettings.Development.json
var connectionString =
    ParseDatabaseUrl(Environment.GetEnvironmentVariable("DATABASE_URL"))
    ?? BuildFromPgVars()
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No connection string found. Set DATABASE_URL in the web service variables on Railway.");

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.EnableDynamicJson();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<AppDbContext>(options =>
    options
        .UseNpgsql(dataSource)
        .UseSnakeCaseNamingConvention());

// === Domain modules ===
builder.Services.AddRentalModule();
builder.Services.AddDecorationModule();
builder.Services.AddShopModule();
builder.Services.AddDictionaryModule();
builder.Services.AddGalleryModule();
builder.Services.AddAdminModule();
builder.Services.AddScoped<FileUploadService>();

// === Identity ===
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "floristshop_auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

var app = builder.Build();

// === Migrations + seed on startup ===
// Runs always — both locally and on Railway.
// Railway = single container, no race conditions, migrations take a moment.
// Seed is idempotent — multiple runs do not create duplicates.
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Migrations — always
    await db.Database.MigrateAsync();

    // Category dictionaries — always (reference data needed in prod)
    await DictionaryModuleExtensions.SeedDictionaryDataAsync(db);

    // Admin seed — always (idempotent, config from env vars or appsettings)
    await IdentitySeed.SeedAsync(scope.ServiceProvider, builder.Configuration);

    // Dev seed data — local only
    if (app.Environment.IsDevelopment())
    {
        await RentalModuleExtensions.SeedRentalDevelopmentDataAsync(db);
        await ShopModuleExtensions.SeedShopDevelopmentDataAsync(db);
    }
}

// === HTTP pipeline ===
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // HSTS skipped — Railway handles HTTPS at its edge proxy.
    // App communicates internally via HTTP (port $PORT).
}

// UseHttpsRedirection skipped for the same reason as HSTS:
// Railway terminates TLS at its load balancer, internally everything goes HTTP.
// Enabling redirect would cause redirect loops.

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// UseStaticFiles handles dynamically added files (photo uploads).
// MapStaticAssets() handles only files known at build time (fingerprinting).
// Both are needed — UseStaticFiles must come first.
app.UseStaticFiles();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

// ============================================================
// Helpers
// ============================================================

/// <summary>
/// Parses DATABASE_URL from Railway (format postgresql://user:pass@host:port/db)
/// into a connection string understood by Npgsql.
/// Returns null if databaseUrl is null or empty.
/// </summary>
static string? ParseDatabaseUrl(string? databaseUrl)
{
    if (string.IsNullOrEmpty(databaseUrl)) return null;

    try
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':', 2);
        var user = Uri.UnescapeDataString(userInfo[0]);
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";
        var database = uri.AbsolutePath.TrimStart('/');

        return $"Host={uri.Host};Port={uri.Port};Database={database};" +
               $"Username={user};Password={password};" +
               $"SSL Mode=Require;Trust Server Certificate=true";
    }
    catch
    {
        return null;
    }
}

/// <summary>
/// Fallback: builds connection string from individual PGHOST, PGPORT, etc. variables.
/// Railway exports these automatically when you add PostgreSQL as a plugin.
/// </summary>
static string? BuildFromPgVars()
{
    var host = Environment.GetEnvironmentVariable("PGHOST");
    if (string.IsNullOrEmpty(host)) return null;

    var port2  = Environment.GetEnvironmentVariable("PGPORT") ?? "5432";
    var db     = Environment.GetEnvironmentVariable("PGDATABASE") ?? "";
    var user   = Environment.GetEnvironmentVariable("PGUSER") ?? "";
    var pass   = Environment.GetEnvironmentVariable("PGPASSWORD") ?? "";

    return $"Host={host};Port={port2};Database={db};" +
           $"Username={user};Password={pass};" +
           $"SSL Mode=Require;Trust Server Certificate=true";
}
