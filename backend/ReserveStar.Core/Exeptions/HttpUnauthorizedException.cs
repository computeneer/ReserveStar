namespace ReserveStar.Core.Exeptions;

public class HttpUnauthorizedException : Exception
{
   public HttpUnauthorizedException() { }

   public HttpUnauthorizedException(string message) : base(message) { }
}