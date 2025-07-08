using HSoft.NetSamples.Api.Data;
using HSoft.NetSamples.Api.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HSoft.NetSamples.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostgreController : ControllerBase
    {
        MyShopDbContext _myShopDbContext;
        ICustomerRepository _customerRepository;

        public PostgreController(MyShopDbContext myShopDbContext, ICustomerRepository customerRepository)
        {
            _myShopDbContext = myShopDbContext;
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await _customerRepository.GetAllAsync();

            return Ok(customers);
        }
    }
}
