using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace HSoft.NetSamples.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        IFeatureManager _featureManager;

        public FeatureController(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            if(await _featureManager.IsEnabledAsync("FeatureA"))
            {
                return Ok("Enabled");
            }
            else
            {
                return Ok("Not enabled");
            }
        }

        [FeatureGate("FeatureA")]
        [HttpGet("GetByFeatureGate")]
        public ActionResult GetByFeatureGate()
        {
            return Ok("Vamos");
        }

        
        [HttpGet("WorkingDay")]
        public async Task<ActionResult> WorkingDay()
        {
            if(await _featureManager.IsEnabledAsync("WorkingDayFeature"))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }

    [FilterAlias("WorkingDayFeatureFilter")]  
    public class WorkingDayFeatureFilter : IFeatureFilter
    {
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            // Acceder a los parámetros desde context.Parameters
            var parameters = context.Parameters.Get<WorkingDayFilterSettings>();

            // Si no hay parámetros definidos o los días permitidos están vacíos, devolvemos false
            if (parameters == null || parameters.AllowedDays == null || !parameters.AllowedDays.Any())
            {
                return Task.FromResult(false);
            }

            // Obtener el día actual
            var today = DateTime.Today.DayOfWeek;

            // Verificar si el día actual está en la lista de días permitidos
            bool isAllowedDay = parameters.AllowedDays.Contains(today);

            return Task.FromResult(isAllowedDay);
        }

        public class WorkingDayFilterSettings
        {
            public List<DayOfWeek> AllowedDays { get; set; }
        }
    }

}
