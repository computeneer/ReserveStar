using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReserveStar.Data.Domain;

namespace ReserveStar.Data.Mapping;

public sealed class TagMapping : BaseTableMapping<Tag>
{
   public override void Configure(EntityTypeBuilder<Tag> builder)
   {
      base.Configure(builder);

      builder.Property(e => e.Name).IsRequired().HasMaxLength(15);

      builder.HasOne(x => x.Company)
             .WithMany()
             .HasForeignKey(x => x.CompanyId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.Cascade);
   }
}