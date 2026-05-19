using FloristShop.Web.Modules.Rental.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloristShop.Web.Modules.Rental.Configuration;

public class RentalReservationConfiguration : IEntityTypeConfiguration<RentalReservation>
{
    public void Configure(EntityTypeBuilder<RentalReservation> builder)
    {
        builder.ToTable("rental_reservations");
        builder.HasKey(x => x.Id);

        // Numer rezerwacji — UNIKALNY index, bo to identyfikator wystawiany klientowi.
        builder.Property(x => x.ReservationNumber)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(x => x.ReservationNumber).IsUnique();

        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.CustomerName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.CustomerEmail).IsRequired().HasMaxLength(200);
        builder.Property(x => x.CustomerPhone).IsRequired().HasMaxLength(50);
        builder.Property(x => x.CustomerNote).HasColumnType("text");
        builder.Property(x => x.AdminNote).HasColumnType("text");

        builder.Property(x => x.TotalPrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(x => x.CreatedAt).IsRequired();

        // === Indeksy ===
        // Najczęstsze zapytania w panelu admina:
        // - lista rezerwacji per status ("pokaż oczekujące")
        // - kalendarz / wyszukiwanie po dacie startu
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.StartDate);

        // === Relacja z pozycjami ===
        // Cascade — usuwając rezerwację, usuwamy jej pozycje (nie ma sensu samodzielne RentalReservationItem).
        builder.HasMany(x => x.Items)
            .WithOne(i => i.RentalReservation)
            .HasForeignKey(i => i.RentalReservationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
