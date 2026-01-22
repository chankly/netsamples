using HSoft.AnotherCasbin.Api.Domain;
using HSoft.AnotherCasbin.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HSoft.AnotherCasbin.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController (ILogger<WeatherForecastController> _logger, IWebHostEnvironment _webHostEnvironment) : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            Sample();

            return Ok();
        }

        private void Sample()
        {
            // Ejemplo de uso
            var userSales = new List<Sale>
            {
                new Sale 
                {
                    SeasonId = Guid.Parse("{223B0657-DB97-4C34-AD8E-AEBB39E26314}"),
                    CompetitionId = Guid.Parse("{223B0657-DB97-4C34-AD8E-AEBB39E26314}"),
                    TeamId = null,
                    MatchId = null,
                    Assets = new List<string> {"video", "stats"}
                },
                new Sale 
                {
                    SeasonId = Guid.Parse("{223B0657-DB97-4C34-AD8E-AEBB39E26314}"),
                    CompetitionId = Guid.Parse("{95256872-0415-4E29-9199-6D2117C45EE0}"),
                    TeamId = null,
                    MatchId = Guid.Parse("{A1FE9732-1A09-4FE0-B3BD-D2F08D9BEC97}"),
                    Assets = new List<string> {"video", "highlights"}
                }
            };

            var filterService = new MatchFilterService(userSales, _webHostEnvironment);
            var allMatches = GetMatchesFromDatabase(); // Obtener todos los partidos

            var filteredMatches = filterService.FilterMatches(allMatches);
        }

        private List<Match> GetMatchesFromDatabase()
        {

            return new List<Match>
            {
                new Match
                {
                    Id = Guid.NewGuid(),
                    SeasonId = Guid.Parse("{223B0657-DB97-4C34-AD8E-AEBB39E26314}"),
                    CompetitionId = Guid.Parse("{223B0657-DB97-4C34-AD8E-AEBB39E26314}"),
                },
                new Match
                {
                    Id = Guid.NewGuid(),
                    SeasonId = Guid.Parse("{223B0657-DB97-4C34-AD8E-AEBB39E26314}"),
                    CompetitionId = Guid.Parse("{223B0657-DB97-4C34-AD8E-AEBB39E26314}"),
                },
                new Match
                {
                    Id = Guid.NewGuid(),
                    SeasonId = Guid.Parse("{223B0657-DB97-4C34-AD8E-AEBB39E26314}"),
                    CompetitionId = Guid.Parse("{95256872-0415-4E29-9199-6D2117C45EE0}"),
                },
                new Match
                {
                    Id = Guid.Parse("A1FE9732-1A09-4FE0-B3BD-D2F08D9BEC97"),
                    SeasonId = Guid.Parse("{223B0657-DB97-4C34-AD8E-AEBB39E26314}"),
                    CompetitionId = Guid.Parse("{95256872-0415-4E29-9199-6D2117C45EE0}"),
                }
            };
        }
    }
}
