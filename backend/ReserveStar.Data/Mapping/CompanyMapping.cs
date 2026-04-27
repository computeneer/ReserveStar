using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReserveStar.Data.Domain;

namespace ReserveStar.Data.Mapping;

public class CompanyMapping : BaseTableMapping<Company>
{
   public override void Configure(EntityTypeBuilder<Company> builder)
   {
      base.Configure(builder);

      builder.Property(e => e.Name).IsRequired().HasMaxLength(127);
      builder.Property(e => e.Description).HasMaxLength(255);
      builder.Property(e => e.LogoUrl);
      builder.Property(e => e.CompanyCode).IsRequired().HasMaxLength(15);

      builder.HasMany(e => e.Tags)
             .WithOne(f => f.Company)
             .HasForeignKey(e => e.CompanyId)
             .OnDelete(DeleteBehavior.Cascade);

      builder.HasIndex(e => e.Name).IsUnique();
      builder.HasIndex(e => e.CompanyCode).IsUnique();

   }
}