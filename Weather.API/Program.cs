using Weather.API;
using Weather.API.Repository;
using Weather.API.Repository.DbConnection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new Config(builder.Configuration);
var dbConnectionFactory = new DbConnectionFactory(config.ConnectionString);

builder.Services.AddSingleton<IConfig>(config);
builder.Services.AddSingleton<IDbConnectionFactory>(dbConnectionFactory);
builder.Services.AddSingleton<IWeatherRepository, WeatherRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();