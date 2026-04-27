namespace ReserveStar.Data.Domain;

public class User : BaseEntity
{
   public required string FirstName { get; set; }
   public required string LastName { get; set; }
   public required string Username { get; set; }
   public required string Email { get; set; }
   public byte[] PasswordSalt { get; set; } = [];
   public byte[] PasswordHash { get; set; } = [];
   public bool IsSuperAdmin { get; set; }
   public bool IsTenantAdmin { get; set; }
   public bool MustChangePassword { get; set; }
   public string RefreshToken { get; set; } = null;
   public DateTime? RefreshTokenExpireDate { get; set; }

   // Relations
   public Guid CompanyId { get; set; }
   public Guid LanguageId { get; set; }

   // Navigation Properties
   public Company Company { get; set; } = null!;
   public Language Language { get; set; } = null!;
}
