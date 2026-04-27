namespace ReserveStar.Core.Authorization.Models;

public record TokenUser(
   Guid UserId,
   string UserName,
   string Email,
   Guid CompanyId,
   bool IsSuperAdmin = false,
   bool IsTenantAdmin = false);
