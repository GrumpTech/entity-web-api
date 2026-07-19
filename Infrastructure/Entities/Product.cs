using EntityWebApi.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    public class Product
    {
        [DtoProperty]
        public int Id { get; set; }

        [DtoProperty, Required]
        public string Name { get; set; } = "";

        [DtoProperty, Required]
        public decimal Price { get; set; }
    }
}