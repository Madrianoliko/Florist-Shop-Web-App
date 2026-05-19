using FloristShop.Web.Modules.Rental.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloristShop.Web.Modules.Rental.Configuration;

/// <summary>
/// Konfiguracja EF Core dla encji RentalItem.
///
/// Trzymamy konfigurację OSOBNO od klasy encji (zamiast atrybutów [Column], [Required] itp.),
/// bo:
/// - Klasa encji pozostaje czysto domenowa, bez "zapachu" persystencji.
/// - Konfiguracja jest w jednym miejscu, łatwo ją czytać i modyfikować.
/// - Łatwiej testować — encję można konstruować w testach bez wciągania EF Core.
///
/// AppDbContext znajdzie tę klasę automatycznie przez ApplyConfigurationsFromAssembly.
/// </summary>
public class RentalItemConfiguration : IEntityTypeConfiguration<RentalItem>
{
    public void Configure(EntityTypeBuilder<RentalItem> builder)
    {
        // Nazwę tabeli ustawiamy jawnie, mimo że pakiet EFCore.NamingConventions
        // i tak zamieniłby "RentalItems" na "rental_items". Robimy to jawnie,
        // żeby intencja była widoczna w jednym miejscu — bez polowania po konfiguracji.
        builder.ToTable("rental_items");

        // Klucz główny (Id z EntityBase) — EF wykryłby to po konwencji, ale deklarujemy explicit.
        builder.HasKey(x => x.Id);

        // === Pola tekstowe ===
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Description — pełen "text" Postgresa (bez limitu). Świadomie, bo może być długi opis.
        builder.Property(x => x.Description)
            .HasColumnType("text");

        // === Kategoria jako string (klucz z dictionary_entries) ===
        // Nie enum — admin może dodawać kategorie bez deployu przez słowniki.
        // Wartości: "Vase", "CandleHolder", "Arch" itd. — takie same jak były w enumie
        // (brak migracji danych — kolumna była już varchar z HasConversion).
        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(100);

        // === Pieniądze ===
        // decimal(10, 2) = max 99999999.99 — z dużym zapasem dla cen rekwizytów.
        builder.Property(x => x.PricePerRental)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(x => x.TotalQuantity)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // === JSONB dla listy zdjęć ===
        // Npgsql provider automatycznie serializuje List<string> do JSONB,
        // dzięki czemu nie musimy mieć osobnej tabeli image_paths.
        // Trade-off: trudniej robić zapytania "znajdź item z konkretnym zdjęciem" —
        // ale takiej potrzeby nigdy nie będziemy mieli.
        builder.Property(x => x.ImagePaths)
            .HasColumnType("jsonb")
            .IsRequired();

        // === Pola audytowe (z EntityBase) ===
        builder.Property(x => x.CreatedAt).IsRequired();
        // UpdatedAt zostaje nullable — nullable z EntityBase

        // === Indeksy ===
        // Najczęstsze zapytanie z katalogu: "pokaż wszystkie aktywne w kategorii X".
        // Indeks na (Category, IsActive) obsłuży to optymalnie.
        builder.HasIndex(x => new { x.Category, x.IsActive });
    }
}
