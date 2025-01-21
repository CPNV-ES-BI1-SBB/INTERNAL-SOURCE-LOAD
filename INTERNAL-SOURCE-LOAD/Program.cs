using INTERNAL_SOURCE_LOAD;
using INTERNAL_SOURCE_LOAD.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient(typeof(IJsonToModelTransformer<>), typeof(JsonToModelTransformer<>));

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
static string GenerateTrainStationSql(TrainStation trainStation)
{
    var sqlStatements = new List<string>();

    foreach (var departure in trainStation.Departures)
    {
        string sql = $"INSERT INTO TrainDepartures (DepartureStationName, DestinationStationName, ViaStationNames, DepartureTime, TrainGroup, TrainLine, Platform, Sector) " +
                     $"VALUES ('{departure.DepartureStationName}', '{departure.DestinationStationName}', '{string.Join(",", departure.ViaStationNames)}', '{departure.DepartureTime:yyyy-MM-dd HH:mm:ss}', " +
                     $"'{departure.Train.G}', '{departure.Train.L}', '{departure.Platform}', {(departure.Sector != null ? $"'{departure.Sector}'" : "NULL")});";

        sqlStatements.Add(sql);
    }

    return string.Join("\n", sqlStatements);
}