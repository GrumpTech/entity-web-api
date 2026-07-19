using EntityWebApi.Core.Attributes;

namespace Infrastructure.Entities
{
    public class Order
    {
        [DtoProperty]
        public int Id { get; set; }

        [DtoProperty]
        public int ClientId { get; set; }

        public Client? Client { get; set; } = null;

        [DtoProperty]
        public DateTime DateTime { get; set; }

        [DtoProperty]
        public ICollection<OrderProduct>? OrderProducts { get; set; } = null;
    }
}