using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreRabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            
            
            const string queueName = "Queue Test";
            
            var publisher = new RabbitMQPublisher(_logger);
    
            
            publisher.PublishStringMessageToQueue("Ricardo Test",queueName);
            
            publisher.PublishBatchMessages(new List<Messages.StringMessage>()
            {
                new() {message = "m1", queueName = queueName},
                new() {message = "m2", queueName = queueName}
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