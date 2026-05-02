using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReserveStar.Data.Domain;

namespace ReserveStar.Data.Mapping;


public sealed class AnnouncementMediaMapping : BaseTableMapping<AnnouncementMedia>
{
   public override void Configure(EntityTypeBuilder<AnnouncementMedia> builder)
   {
      base.Configure(builder);

      builder.HasOne(e => e.Announcement)
          .WithMany(e => e.Medias)
          .HasForeignKey(e => e.AnnouncementId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

      builder.HasOne(e => e.Media)
          .WithMany()
          .HasForeignKey(e => e.MediaId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Restrict);

      builder.HasIndex(e => new { e.AnnouncementId, e.MediaId })
          .IsUnique();
   }
}