namespace HSoft.NetSamples.Api.Domain.Entities
{
    public class OrderEntity
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }
        public Address Address { get; set; }
        public List<OrderDetailEntity> OrderDetails { get; set; }
        public OrderStateType OrderState { get; set; }
    }
}
