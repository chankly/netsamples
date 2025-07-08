namespace HSoft.NetSamples.Api.Domain.Entities
{
    public class CustomerEntity : BaseEntity
    {
        public string Name { get; set; }
        public List<OrderEntity> Orders { get; set; }
    }
}
