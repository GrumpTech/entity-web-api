using System.ComponentModel.DataAnnotations;

namespace Tests.Shared.Entities
{
    public class CompositeKeyChild
    {
        [Key]
        public int Key { get; set; }
        [Key]
        public string? Key2 { get; set; }

        public int Regular1 { get; set; }
        public string? Regular2 { get; set; }
    }
}
