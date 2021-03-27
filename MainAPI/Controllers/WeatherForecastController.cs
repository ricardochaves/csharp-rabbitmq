using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoreRabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MainAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly RabbitMqPublisher _publisher;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, RabbitMqPublisher publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {


            const string queueName = "Queue Test";

            var x = Activity.Current;

            var objToSend = new Dictionary<string, string> {{"msg", "Ricardo Test"}};
            var message = new RabbitMqMessage(objToSend, queueName);

            _publisher.PublishMessage(message);

            _publisher.PublishBatchMessages(new List<RabbitMqMessage>()
            {
                new(objToSend,  queueName) ,
                new(objToSend,  queueName)
            });


            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}
