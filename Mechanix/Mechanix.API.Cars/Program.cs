using Mechanix.API.Core.Auth.Requirements;
using Mechanix.API.Core.Data;
using Mechanix.API.Core.Data.Models;
using Mechanix.API.Core.Data.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

#region Configure

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();


string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins,
      builder => {
          builder.WithOrigins("*");
      });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://dev-4pw2cdab.us.auth0.com/";
    options.Audience = "https://mechanix.com";
});

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("mechanix:read-write", policy => policy.Requirements.Add(new HasScopeRequirement("mechanix:read-write", builder.Configuration["Auth0:Domain"])));
});

builder.Services.AddSingleton<IDbConnection, DbConnection>();
builder.Services.AddSingleton<IService<Car>, CarService>();
builder.Services.AddSingleton<IService<Part>, PartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

#endregion

#region Map routes

#region Car data

app.MapGet("/cars", async (IService<Car> service) => await service.GetAllAsync()).RequireAuthorization("mechanix:read-write");
app.MapGet("/cars/{id}", async (IService<Car> service, string id) => await service.GetAsync(id)).RequireAuthorization("mechanix:read-write");
app.MapPost("/cars", async (IService<Car> service, Car car) => await service.CreateAsync(car)).RequireAuthorization("mechanix:read-write");
app.MapPut("/cars", async (IService<Car> service, Car car) => await service.UpdateAsync(car)).RequireAuthorization("mechanix:read-write");
app.MapDelete("/cars/{id}", async (IService<Car> service, string id) => await service.RemoveAsync(id)).RequireAuthorization("mechanix:read-write");

#endregion

#endregion

app.Run();