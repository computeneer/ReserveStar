using ReserveStar.Helper.Enums;

namespace ReserveStar.Data.Domain;

public class Media : BaseEntity
{
   public string FileName { get; set; }
   public string BucketName { get; set; }
   public MediaType MediaType { get; set; }
   public string ContentType { get; set; }
}