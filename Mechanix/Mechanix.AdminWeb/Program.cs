using Mechanix.Core.Data;
using Mechanix.Core.Data.Models;
using Mechanix.Core.Data.Services;

#region Configuration

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IDbConnection, DbConnection>();
builder.Services.AddSingleton<IService<Car>, CarService>();
builder.Services.AddSingleton<IService<Part>, PartService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapFallbackToFile("index.html"); ;

#endregion

#region Sample data

string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

app.MapGet("/WeatherForecast", () => {
    return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    })
    .ToArray();
});

#endregion

#region Car data

app.MapGet("/cars", async (IService<Car> service) => await service.GetAllAsync());
app.MapGet("/cars/{id}", async (IService<Car> service, string id) => await service.GetAsync(id));
app.MapPost("/cars", async (IService<Car> service, Car car) => await service.CreateAsync(car));
app.MapPut("/cars", async (IService<Car> service, Car car) => await service.UpdateAsync(car));
app.MapDelete("/cars/{id}", async (IService<Car> service, string id) => await service.RemoveAsync(id));

#endregion

app.Run();

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}
