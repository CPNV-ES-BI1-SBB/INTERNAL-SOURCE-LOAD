using System.Text.Json;

namespace INTERNAL_SOURCE_LOAD;

public class TrainJsonToSqlTransformer : IJsonToSqlTransformer
{
    public bool CanHandle(JsonElement jsonData)
    {
        // Check if the JSON has the required structure
        if (!jsonData.TryGetProperty("name", out var name) || name.ValueKind != JsonValueKind.String)
            return false;

        if (!jsonData.TryGetProperty("departures", out var departures) || departures.ValueKind != JsonValueKind.Array)
            return false;

        foreach (var departure in departures.EnumerateArray())
        {
            if (!departure.TryGetProperty("departureStationName", out var departureStationName) || departureStationName.ValueKind != JsonValueKind.String)
                return false;

            if (!departure.TryGetProperty("destinationStationName", out var destinationStationName) || destinationStationName.ValueKind != JsonValueKind.String)
                return false;

            if (!departure.TryGetProperty("departureTime", out var departureTime) || departureTime.ValueKind != JsonValueKind.String)
                return false;

            if (!departure.TryGetProperty("train", out var train) || train.ValueKind != JsonValueKind.Object)
                return false;

            if (!train.TryGetProperty("g", out var g) || g.ValueKind != JsonValueKind.String)
                return false;

            // Optional checks for nullable fields can be added if needed
        }

        return true;
    }

    public string Transform(JsonElement jsonData)
    {
         TrainStation trainStation = JsonSerializer.Deserialize<TrainStation>(jsonData.GetRawText(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (trainStation == null)
            {
                throw new ArgumentException("Invalid JSON format for TrainStation.");
            }

            // Step 2: Convert the object to SQL
            return GenerateSql(trainStation);
    }

    private string GenerateSql(TrainStation trainStation)
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
}