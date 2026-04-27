using System.Net;

namespace ReserveStar.Helper.Extensions;

public static class HttpStatusCodeExtensions
{
   public static string ToIntString(this HttpStatusCode statusCode)
   {
      return ((int)statusCode).ToString();
   }
}