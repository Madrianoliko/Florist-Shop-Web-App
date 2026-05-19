using FloristShop.Web.Modules.Decoration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloristShop.Web.Modules.Decoration.Configuration;

public class DecorationInquiryConfiguration : IEntityTypeConfiguration<DecorationInquiry>
{
    public void Configure(EntityTypeBuilder<DecorationInquiry> builder)
    {
        builder.ToTable("decoration_inquiries");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.InquiryNumber)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(x => x.InquiryNumber).IsUnique();

        builder.Property(x => x.EventType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.EventDate).IsRequired();
        builder.Property(x => x.Location).IsRequired().HasMaxLength(300);
        builder.Property(x => x.Description).IsRequired().HasColumnType("text");

        builder.Property(x => x.BudgetEstimate).HasPrecision(10, 2);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.CustomerName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.CustomerEmail).IsRequired().HasMaxLength(200);
        builder.Property(x => x.CustomerPhone).IsRequired().HasMaxLength(50);

        builder.Property(x => x.AdminNote).HasColumnType("text");
        builder.Property(x => x.FinalPrice).HasPrecision(10, 2);

        builder.Property(x => x.CreatedAt).IsRequired();

        // Indeksy — admin filtruje po statusie ("co do zrobienia") i po dacie wydarzenia (planowanie).
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.EventDate);
    }
}
