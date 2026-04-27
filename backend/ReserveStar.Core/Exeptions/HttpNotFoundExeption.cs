namespace ReserveStar.Core.Exeptions;

public class HttpNotFoundExeption : Exception
{
   public HttpNotFoundExeption() { }
   public HttpNotFoundExeption(string message) : base(message) { }
}