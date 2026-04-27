namespace ReserveStar.Core.Exeptions;

public class AuthenticationException : Exception
{
   public AuthenticationException() { }
   public AuthenticationException(string message) : base(message) { }
}