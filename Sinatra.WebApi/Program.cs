using Microsoft.EntityFrameworkCore;
using Sinatra.WebApi.Data.Context;
using Sinatra.WebApi.Helpers.Authorization;
using Sinatra.WebApi.Services;
using System.Text.Json.Serialization;
using Sinatra.WebApi.Helpers.Configuration;
using Sinatra.WebApi.Helpers.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opts => { opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database.
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("SinatraDb"));

// Add service layer.
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JwtConfiguration"));
builder.Services.AddSingleton<IJwtUtils, JwtUtils>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserContext>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

