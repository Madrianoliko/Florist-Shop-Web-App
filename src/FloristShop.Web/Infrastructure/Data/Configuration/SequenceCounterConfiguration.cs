using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloristShop.Web.Infrastructure.Data.Configuration;

/// <summary>
/// Konfiguracja EF Core dla SequenceCounter.
///
/// Klucz złożony (Type, Year) pełni rolę ograniczenia UNIQUE — jest PK tabeli.
/// To właśnie ten klucz używany jest w klauzuli ON CONFLICT (type, year) w UPSERT-cie.
/// </summary>
public class SequenceCounterConfiguration : IEntityTypeConfiguration<SequenceCounter>
{
    public void Configure(EntityTypeBuilder<SequenceCounter> builder)
    {
        builder.ToTable("sequence_counters");

        // Klucz złożony — ORDER ma znaczenie (Postgres indeksuje w tej kolejności).
        // Type jako pierwszy — szybsze gdy filtrujemy po samym Type.
        builder.HasKey(x => new { x.Type, x.Year });

        builder.Property(x => x.Type)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.Year)
            .IsRequired();

        builder.Property(x => x.LastValue)
            .IsRequired()
            .HasDefaultValue(0);
    }
}
