using FloristShop.Web.Modules.Rental.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloristShop.Web.Modules.Rental.Configuration;

public class RentalReservationItemConfiguration : IEntityTypeConfiguration<RentalReservationItem>
{
    public void Configure(EntityTypeBuilder<RentalReservationItem> builder)
    {
        builder.ToTable("rental_reservation_items");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quantity).IsRequired();

        builder.Property(x => x.UnitPriceSnapshot)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(x => x.CreatedAt).IsRequired();

        // === Relacja z RentalItem (encja katalogu) ===
        // Restrict — NIE pozwalamy usunąć RentalItem, jeśli są historyczne rezerwacje.
        // Chroni dane historyczne. Aby "usunąć" rekwizyt z katalogu — ustaw IsActive=false.
        builder.HasOne(x => x.RentalItem)
            .WithMany()
            .HasForeignKey(x => x.RentalItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacja z RentalReservation jest skonfigurowana w RentalReservationConfiguration (Cascade).
    }
}
