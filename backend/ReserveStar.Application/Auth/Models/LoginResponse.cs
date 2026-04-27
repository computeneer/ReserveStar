namespace ReserveStar.Application.Auth.Models;

public sealed record LoginResponse(string AccessToken, DateTime ValidTo);