using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

namespace INTERNAL_SOURCE_LOAD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoadController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;

    public LoadController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [HttpPost("{modelType}")]
    public IActionResult Post(string modelType, [FromBody] JsonElement jsonData)
    {
        if (jsonData.ValueKind == JsonValueKind.Undefined || jsonData.ValueKind == JsonValueKind.Null)
        {
            return BadRequest("Invalid JSON payload.");
        }

        try
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly(); // Gets the current assembly

            Type targetType = currentAssembly.GetTypes()
                                        .FirstOrDefault(t => t.Namespace == "INTERNAL_SOURCE_LOAD.Models" && t.Name == modelType);

            var transformerType = typeof(IJsonToModelTransformer<>).MakeGenericType(targetType);

            dynamic transformer = _serviceProvider.GetService(transformerType);
            if (transformer == null)
            {
                return BadRequest($"No transformer found for model type: {modelType}");
            }

            // Transform JSON into the specified model type
            var model = transformer.Transform(jsonData);

            // Optionally, return the model or success message
            return StatusCode(StatusCodes.Status201Created, $"Data transformed successfully for model type: {modelType}");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error processing data: {ex.Message}");
        }
    }
}
