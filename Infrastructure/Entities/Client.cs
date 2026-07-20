using System.ComponentModel.DataAnnotations;
using EntityWebApi.Core.Attributes;
using Infrastructure.Attributes;

namespace Infrastructure.Entities
{
    public class Client
    {
        [DtoProperty]
        public int Id { get; set; }

        [DtoProperty, PatchDtoProperty, Required]
        public string Name { get; set; } = "";

        [DtoProperty, PatchDtoProperty, Required]
        public DateTime DateOfBirth { get; set; }

        public ICollection<Order>? Orders { get; set; } = null;
    }
}
