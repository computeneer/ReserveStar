using MediatR;

namespace ReserveStar.Helper.Api;

public class BaseRequest<T> : IRequest<T>
{
   public Guid UserId { get; set; }
   public Guid LanguageId { get; set; }
   public Guid CompanyId { get; set; }
}