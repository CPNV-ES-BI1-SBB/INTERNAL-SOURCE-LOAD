using INTERNAL_SOURCE_LOAD;
using INTERNAL_SOURCE_LOAD.Controllers;
using INTERNAL_SOURCE_LOAD.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NBench;
using System.Text.Json;

namespace StressTest
{
    internal class LoadControllerStressTest
    {
        private Counter? _counter;
        private LoadController? _controller;
        private JsonElement _validJson;

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            _counter = context.GetCounter("MyCounter");

            // Load configuration (e.g., from appsettings.json)
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Configure AppSettings
            var appSettings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(appSettings);

            // Initialize real dependencies
            var serviceProvider = new ServiceCollection()
                .AddTransient(typeof(IJsonToModelTransformer<>), typeof(JsonToModelTransformer<>))
                .BuildServiceProvider();

            // Initialize the database executor with a real connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var sqlExecutor = new MariaDbExecutor(connectionString);

            // Initialize the controller with real dependencies
            _controller = new LoadController(serviceProvider, Options.Create(appSettings), sqlExecutor);

            // Load valid JSON from the /data folder
            _validJson = JsonDocument.Parse(File.ReadAllText("Lausanne.json")).RootElement;
        }

        [PerfBenchmark(
            NumberOfIterations = 1, // Run the test once (duration is controlled by RunTimeMilliseconds)
            RunMode = RunMode.Throughput, // Measure throughput (requests per second)
            RunTimeMilliseconds = 60000, // Run for 1 minute (60,000 milliseconds)
            TestMode = TestMode.Measurement)] // Use Measurement mode to find the breakpoint
        [CounterThroughputAssertion("MyCounter", MustBe.GreaterThan, 0)] // Ensure at least some requests are processed
        public void Post_ValidJson_ReturnsOk()
        {

            // Arrange
            Console.WriteLine("Test");
            // Act
            var result = _controller.Post(_validJson);
            Console.WriteLine("OIK");

            _counter.Increment();
        }
    }
}
