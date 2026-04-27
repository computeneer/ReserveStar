namespace ReserveStar.Data.Domain;

public class Company : BaseEntity
{
   public required string Name { get; set; }
   public string Description { get; set; } = null;
   public string LogoUrl { get; set; } = null;
   public required string CompanyCode { get; set; }
}
