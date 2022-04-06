using Mechanix.API.Core.Auth.Handlers;
using Mechanix.API.Core.Auth.Requirements;
using Mechanix.API.Core.Data;
using Mechanix.API.Core.Data.Models;
using Mechanix.API.Core.Data.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

#region Configure

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();


string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins,
      builder => {
          builder.WithOrigins("*");
          builder.WithHeaders("*");
          builder.WithMethods("*");
      });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Authority"];
    options.Audience = builder.Configuration["Auth0:Audience"];
});

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("read:cars", policy => policy.Requirements.Add(new HasScopeRequirement("read:cars", builder.Configuration["Auth0:Domain"])));
});

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

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

//app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

#endregion

#region Map routes

app.MapGet("/cars", async (IService<Car> service) => await service.GetAllAsync()).RequireAuthorization("read:cars");
app.MapGet("/cars/{id}", async (IService<Car> service, string id) => await service.GetAsync(id)).RequireAuthorization("read:cars");
app.MapPost("/cars", async (IService<Car> service, Car car) => await service.CreateAsync(car)).RequireAuthorization("read:cars");
app.MapPut("/cars", async (IService<Car> service, Car car) => await service.UpdateAsync(car)).RequireAuthorization("read:cars");
app.MapDelete("/cars/{id}", async (IService<Car> service, string id) => await service.RemoveAsync(id)).RequireAuthorization("read:cars");

#endregion

app.Run();