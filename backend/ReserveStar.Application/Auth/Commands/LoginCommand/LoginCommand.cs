using ReserveStar.Application.Auth.Models;
using ReserveStar.Helper.Api;

namespace ReserveStar.Application.Auth.Commands.LoginCommand;

public sealed class LoginCommand() : BaseRequest<LoginResponse>
{
   public string CompanyCode { get; set; }
   public string Username { get; set; }
   public string Password { get; set; }
}