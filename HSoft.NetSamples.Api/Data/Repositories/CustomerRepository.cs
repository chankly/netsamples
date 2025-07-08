using HSoft.NetSamples.Api.Domain.Entities;

namespace HSoft.NetSamples.Api.Data.Repositories
{
    public interface ICustomerRepository : IGenericRepository<CustomerEntity> { }

    public class CustomerRepository : GenericRepository<CustomerEntity>, ICustomerRepository
    {
        public CustomerRepository(MyShopDbContext myShopDbContext) : base(myShopDbContext)
        {
            
        }
    }
}
