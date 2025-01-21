using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace INTERNAL_SOURCE_LOAD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoadController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AppSettings _appSettings;

    public LoadController(IServiceProvider serviceProvider, IOptions<AppSettings> appSettings)
    {
        _serviceProvider = serviceProvider;
        _appSettings = appSettings.Value;
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

            // Return success response
            return StatusCode(StatusCodes.Status201Created, $"Data transformed successfully into model type: {_appSettings.DefaultModel}");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error processing data: {ex.Message}");
        }
    }
}
