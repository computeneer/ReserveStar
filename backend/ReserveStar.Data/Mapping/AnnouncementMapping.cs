using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReserveStar.Data.Domain;

namespace ReserveStar.Data.Mapping;

public class AnnouncementMapping : BaseTableMapping<Announcement>
{
   public override void Configure(EntityTypeBuilder<Announcement> builder)
   {
      base.Configure(builder);

      builder.Property(e => e.Title).IsRequired().HasMaxLength(31);
      builder.Property(e => e.Content).IsRequired();

      builder.HasMany(e => e.Tags)
       .WithMany(e => e.Announcements)
       .UsingEntity<AnnouncementTag>(
           j => j.HasOne(pt => pt.Tag).WithMany().HasForeignKey(pt => pt.TagId),
           j => j.HasOne(pt => pt.Announcement).WithMany().HasForeignKey(pt => pt.AnnouncementId),
           j => { j.HasKey(t => new { t.AnnouncementId, t.TagId }); }
       );

      builder.HasMany(e => e.Medias)
         .WithOne(e => e.Announcement)
         .HasForeignKey(e => e.AnnouncementId)
         .OnDelete(DeleteBehavior.Cascade);


      builder.HasOne(e => e.Company)
             .WithMany(e => e.Announcements)
             .HasForeignKey(f => f.CompanyId).OnDelete(DeleteBehavior.Cascade);

      builder.HasIndex(f => f.CompanyId);
   }
}