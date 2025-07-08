using HSoft.NetSamples.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSoft.NetSamples.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MigrationController : ControllerBase
    {
        MyShopDbContext _myShopDbContext;
        ILogger<MigrationController> _logger;

        public MigrationController(MyShopDbContext myShopDbContext, ILogger<MigrationController> logger)
        {
            _myShopDbContext = myShopDbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Check()
        {
            var pendingMigrations = await _myShopDbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations?.Any() == true)
            {
                _logger.LogInformation("Appling migrations");
                await _myShopDbContext.Database.MigrateAsync();
            }

            return Ok();
        }
    }
}
