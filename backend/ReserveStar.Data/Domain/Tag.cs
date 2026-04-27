namespace ReserveStar.Data.Domain;

public class Tag : BaseEntity
{
   public required string Name { get; set; }
   public Guid? CompanyId { get; set; }

   // Relations
   public Company Company { get; set; } = null;
   public List<Announcement> Announcements { get; set; } = [];
}
