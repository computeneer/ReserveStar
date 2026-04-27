namespace ReserveStar.Data.Domain;

public class Announcement : BaseEntity
{
   public required string Title { get; set; }
   public string Content { get; set; }
   public List<string> Images { get; set; } = [];
   public required Guid CompanyId { get; set; }

   // Relations
   public Company Company { get; set; }
   public List<Tag> Tags { get; set; } = [];
}
