using INTERNAL_SOURCE_LOAD.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Text.Json;

namespace INTERNAL_SOURCE_LOAD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoadController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AppSettings _appSettings;
    private readonly IDatabaseExecutor _sqlExecutor;

    public LoadController(IServiceProvider serviceProvider, IOptions<AppSettings> appSettings, IDatabaseExecutor sqlExecutor)
    {
        _serviceProvider = serviceProvider;
        _appSettings = appSettings.Value;
        _sqlExecutor = sqlExecutor;
    }

    [HttpPost]
    public IActionResult Post([FromBody] JsonElement jsonData)
    {
        if (jsonData.ValueKind == JsonValueKind.Undefined || jsonData.ValueKind == JsonValueKind.Null)
        {
            return BadRequest("Invalid JSON payload.");
        }

        try
        {
            // Resolve the target type from the configuration
            var targetType = Type.GetType(_appSettings.DefaultModel);
            if (targetType == null)
            {
                return BadRequest($"Model type '{_appSettings.DefaultModel}' not found.");
            }

            var transformerType = typeof(IJsonToModelTransformer<>).MakeGenericType(targetType);
            dynamic transformer = _serviceProvider.GetService(transformerType);
            if (transformer == null)
            {
                return BadRequest($"No transformer found for model type: {_appSettings.DefaultModel}");
            }

            // Transform JSON into the specified model type
            var model = transformer.Transform(jsonData);

            if (model == null)
            {
                return BadRequest("Transformation resulted in a null model.");
            }

            // Generate SQL queries
            string tableName = targetType.Name;
            var sqlQueries = SqlInsertGenerator.GenerateInsertQueries(tableName, model);

            // Execute each query
            foreach (var query in sqlQueries)
            {
                _sqlExecutor.Execute(query);
            }

            return StatusCode(StatusCodes.Status201Created, $"Data inserted into table: {tableName}");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error processing data: {ex.Message}");
        }
    }

}
