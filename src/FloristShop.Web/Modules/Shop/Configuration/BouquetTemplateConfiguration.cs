using FloristShop.Web.Modules.Shop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloristShop.Web.Modules.Shop.Configuration;

public class BouquetTemplateConfiguration : IEntityTypeConfiguration<BouquetTemplate>
{
    public void Configure(EntityTypeBuilder<BouquetTemplate> builder)
    {
        builder.ToTable("bouquet_templates");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasColumnType("text");

        builder.Property(x => x.BasePrice)
            .IsRequired()
            .HasPrecision(10, 2);

        // Klucz z dictionary_entries (type = "bouquet_category"), np. "Wedding", "Birthday".
        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ImagePaths)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => new { x.Category, x.IsActive });
    }
}
