using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace HSoft.NetSamples.Api.IntegrationTest
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UnitTest1(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task Test1()
        {
            //Arrage
            var client = _factory.CreateClient();

            //Act
            var result = await client.GetFromJsonAsync<List<Domain.Entities.CustomerEntity>>("/api/Postgre");

            //Assert
        }
    }
}