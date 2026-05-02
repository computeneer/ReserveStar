using ReserveStar.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddCommonServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();

app.UseGlobalErrorHandler();
app.UseAuthenticationMiddleware();
app.UseCustomAuthenticationMiddleware();
app.AddCommonServices();

app.MapControllers();

app.Run();
