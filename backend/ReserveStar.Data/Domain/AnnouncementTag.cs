namespace ReserveStar.Data.Domain;

public class AnnouncementTag : BaseEntity
{
   public Guid AnnouncementId { get; set; }
   public Guid TagId { get; set; }

   public Announcement Announcement { get; set; }
   public Tag Tag { get; set; }
}