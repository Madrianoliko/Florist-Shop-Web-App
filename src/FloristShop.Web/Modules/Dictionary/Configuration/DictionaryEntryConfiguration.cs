using FloristShop.Web.Modules.Dictionary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloristShop.Web.Modules.Dictionary.Configuration;

public class DictionaryEntryConfiguration : IEntityTypeConfiguration<DictionaryEntry>
{
    public void Configure(EntityTypeBuilder<DictionaryEntry> builder)
    {
        builder.ToTable("dictionary_entries");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).UseIdentityColumn(); // int auto-increment

        builder.Property(x => x.Type).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Key).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Label).IsRequired().HasMaxLength(200);
        builder.Property(x => x.SortOrder).HasDefaultValue(0);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        // Unikalny klucz słownikowy — (Type, Key) musi być unikalny
        builder.HasIndex(x => new { x.Type, x.Key }).IsUnique();
        // Szybkie pobieranie wszystkich wpisów danego typu
        builder.HasIndex(x => new { x.Type, x.IsActive });
    }
}
