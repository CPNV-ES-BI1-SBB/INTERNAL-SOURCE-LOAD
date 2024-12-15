using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace INTERNAL_SOURCE_LOAD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadController : ControllerBase
    {
        private readonly IEnumerable<IJsonToSqlTransformer> _transformers;

        public LoadController(IEnumerable<IJsonToSqlTransformer> transformers)
        {
            _transformers = transformers;
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
                var transformer = _transformers.FirstOrDefault(t => t.CanHandle(jsonData));

                if (transformer == null)
                {
                    return BadRequest("No suitable transformer found for the provided data.");
                }

                string sqlQuery = transformer.Transform(jsonData);

                // Execute SQL query (optional)
                // _sqlExecutor.Execute(sqlQuery);

                return StatusCode(StatusCodes.Status201Created, "Data loaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error loading data: {ex.Message}");
            }
        }
    }

}