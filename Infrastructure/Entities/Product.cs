using EntityWebApi.Core.Attributes;
using Infrastructure.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    public class Product
    {
        [DtoProperty]
        public int Id { get; set; }

        [DtoProperty, PatchDtoProperty, Required]
        public string Name { get; set; } = "";

        [DtoProperty, PatchDtoProperty, Required]
        public decimal Price { get; set; }
    }
}