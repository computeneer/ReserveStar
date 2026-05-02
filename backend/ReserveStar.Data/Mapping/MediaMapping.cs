using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReserveStar.Data.Domain;
using ReserveStar.Helper.Enums;

namespace ReserveStar.Data.Mapping;

public sealed class MediaMapping : BaseTableMapping<Media>
{
   public override void Configure(EntityTypeBuilder<Media> builder)
   {
      base.Configure(builder);

      builder.Property(e => e.FileName).IsRequired();
      builder.Property(e => e.BucketName);
      builder.Property(e => e.ContentType);

      builder.Property(e => e.MediaType)
        .HasConversion(
            v => v.ToString(),
            v => (MediaType)Enum.Parse(typeof(MediaType), v));

      builder.HasIndex(e => e.MediaType);
   }
}