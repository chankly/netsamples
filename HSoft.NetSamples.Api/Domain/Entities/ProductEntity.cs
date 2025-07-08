using HSoft.NetSamples.Api.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace HSoft.NetSamples.Api.Domain.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CategoryType CategoryType { get; set; }
        public int Stock { get; set; }
    }
}
