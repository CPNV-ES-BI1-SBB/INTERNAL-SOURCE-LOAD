using System.Text.Json;
using Aspose.Cells;
using INTERNAL_SOURCE_LOAD;
using INTERNAL_SOURCE_LOAD.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework.Legacy;


namespace INTERNAL_SOURCE_LOAD_TEST
{
  [TestFixture]
  public class JsonToModelTransformerTests
  {
        private JsonToModelTransformer<TestModel> _transformer;

        [SetUp]
        public void Setup()
        {
            _transformer = new JsonToModelTransformer<TestModel>();
        }

        [Test]
        public void Transform_ValidJson_ReturnsDeserializedObject()
        {
            // Arrange
            var validJson = JsonDocument.Parse("{'name':'TestName','age':30}").RootElement;

            // Act
            var result = _transformer.Transform(validJson);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("TestName", result.Name);
            Assert.AreEqual(30, result.Age);
        }

        [Test]
        public void Transform_InvalidJson_ThrowsArgumentException()
        {
            // Arrange
            var invalidJson = JsonDocument.Parse("null").RootElement;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _transformer.Transform(invalidJson));
            Assert.AreEqual("Invalid JSON payload.", ex.Message);
        }

        [Test]
        public void Transform_EmptyJson_ThrowsArgumentException()
        {
            // Arrange
            var emptyJson = JsonDocument.Parse("{}\").RootElement;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _transformer.Transform(emptyJson));
            Assert.That(ex.Message.Contains("Failed to deserialize JSON"));
        }

        public class TestModel
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
