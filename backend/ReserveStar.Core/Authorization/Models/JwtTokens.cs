namespace ReserveStar.Core.Authorization.Models;

public record JwtTokens(string AccessToken, string RefreshToken, DateTime ValidTo);
