using EntityWebApi.Core.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities
{
    [PrimaryKey(nameof(OrderId), nameof(ProductId))]
    public class OrderProduct
    {
        [DtoProperty]
        public int OrderId { get; set; }

        public Order? Order { get; set; } = null;

        [DtoProperty]
        public int ProductId { get; set; }

        public Product? Product { get; set; } = null;

        [DtoProperty]
        public int Amount { get; set; }

        [DtoProperty]
        public decimal Price { get; set; }

    }
}