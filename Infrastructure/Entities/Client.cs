using System.ComponentModel.DataAnnotations;
using EntityWebApi.Core.Attributes;

namespace Infrastructure.Entities
{
    public class Client
    {
        [DtoProperty]
        public int Id { get; set; }

        [DtoProperty, Required]
        public string Name { get; set; } = "";

        [DtoProperty, Required]
        public DateTime DateOfBirth { get; set; }

        public ICollection<Order>? Orders { get; set; } = null;
    }
}
