using System.Text.Json;
using INTERNAL_SOURCE_LOAD;
using INTERNAL_SOURCE_LOAD.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework.Legacy;


namespace INTERNAL_SOURCE_LOAD_TEST
{
    [TestFixture]
    public class TrainJsonToSqlTransformerTests
    {
        private TrainJsonToSqlTransformer _transformer;
        private LoadController _controller;

        [SetUp]
        public void Setup()
        {
            _transformer = new TrainJsonToSqlTransformer();
            List<IJsonToSqlTransformer> transformers = new List<IJsonToSqlTransformer> { new TrainJsonToSqlTransformer() };
            _controller = new LoadController(transformers);
        }

    }
}   
