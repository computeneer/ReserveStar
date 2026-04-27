namespace ReserveStar.Data.Domain;

public class MessageTemplate : BaseEntity
{
   public required string TemplateKey { get; set; }
   public Guid LanguageId { get; set; }
   public required string Subject { get; set; }
   public required string Body { get; set; }
   public Guid CompanyId { get; set; }

   // Navigation Properties
   public Company Company { get; set; } = null;
   public Language Language { get; set; } = null;
}
