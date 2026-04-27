namespace ReserveStar.Data.Domain;

public class Announcement : BaseEntity
{
   public required string Title { get; set; }
   public string Content { get; set; }
   public string[] Images { get; set; } = [];
   public Guid[] Tags { get; set; }
   public required Guid CompanyId { get; set; }
}
