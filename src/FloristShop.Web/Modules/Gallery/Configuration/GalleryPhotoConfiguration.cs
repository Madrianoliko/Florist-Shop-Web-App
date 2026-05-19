using FloristShop.Web.Modules.Gallery.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloristShop.Web.Modules.Gallery.Configuration;

public class GalleryPhotoConfiguration : IEntityTypeConfiguration<GalleryPhoto>
{
    public void Configure(EntityTypeBuilder<GalleryPhoto> builder)
    {
        builder.ToTable("gallery_photos");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(200);
        builder.Property(x => x.Description).HasColumnType("text");
        builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Category).IsRequired().HasMaxLength(100);
        builder.Property(x => x.DisplayOrder).HasDefaultValue(0);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => new { x.IsActive, x.DisplayOrder });
    }
}
