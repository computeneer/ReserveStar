namespace ReserveStar.Data.Domain;

public class AnnouncementMedia : BaseEntity
{
   public Guid AnnouncementId { get; set; }
   public Guid MediaId { get; set; }

   public Announcement Announcement { get; set; }
   public Media Media { get; set; }
}