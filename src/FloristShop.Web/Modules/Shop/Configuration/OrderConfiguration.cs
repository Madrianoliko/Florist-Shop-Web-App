using FloristShop.Web.Modules.Shop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloristShop.Web.Modules.Shop.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(x => x.OrderNumber).IsUnique();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        // === Dane kontaktowe ===
        builder.Property(x => x.CustomerName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.CustomerEmail).IsRequired().HasMaxLength(200);
        builder.Property(x => x.CustomerPhone).IsRequired().HasMaxLength(50);

        // === Dostawa ===
        builder.Property(x => x.DeliveryMethod)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.Property(x => x.DeliveryAddress).HasMaxLength(500);
        builder.Property(x => x.DeliveryDate).IsRequired();

        // === Notatki ===
        builder.Property(x => x.CustomerNote).HasColumnType("text");
        builder.Property(x => x.AdminNote).HasColumnType("text");

        // === Kwoty ===
        builder.Property(x => x.Subtotal).IsRequired().HasPrecision(10, 2);
        builder.Property(x => x.DeliveryFee).IsRequired().HasPrecision(10, 2);
        builder.Property(x => x.TotalPrice).IsRequired().HasPrecision(10, 2);

        // === Płatność ===
        builder.Property(x => x.PaymentStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.CreatedAt).IsRequired();

        // === Indeksy ===
        // Status: panel admina "co do zrobienia"
        // DeliveryDate: planowanie produkcji "co na dzisiaj"
        // PaymentStatus: filtr "nieopłacone"
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.DeliveryDate);
        builder.HasIndex(x => x.PaymentStatus);

        // === Relacja z pozycjami — Cascade ===
        builder.HasMany(x => x.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
