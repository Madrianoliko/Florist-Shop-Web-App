using FloristShop.Web.Modules.Shop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloristShop.Web.Modules.Shop.Configuration;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.BouquetNameSnapshot)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Quantity).IsRequired();

        builder.Property(x => x.UnitPriceSnapshot)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(x => x.CreatedAt).IsRequired();

        // === Relacja z BouquetTemplate — Restrict ===
        // NIE pozwalamy usunąć szablonu jeśli są historyczne zamówienia.
        // Snapshot name + price w OrderItem chronią dane wyświetlane —
        // ale referencja FK pozostaje, żebyśmy mogli pokazać "zobacz w katalogu"
        // dla nadal aktywnych szablonów.
        builder.HasOne(x => x.BouquetTemplate)
            .WithMany()
            .HasForeignKey(x => x.BouquetTemplateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
