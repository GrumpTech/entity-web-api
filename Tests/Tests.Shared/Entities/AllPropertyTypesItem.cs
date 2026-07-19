using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tests.Shared.Entities
{
    public class AllPropertyTypesItem
    {
        [Key]
        [Column(Order = 1)]
        public int Key { get; set; }
        [Key]
        [Column(Order = 1)]
        public string? Key2 { get; set; }

        public int Regular1 { get; set; }
        public string? Regular2 { get; set; }

        [ForeignKey("CompositeKeyChild")]
        public int ForeignKey1 { get; set; }
        [ForeignKey("CompositeKeyChild")]
        public int ForeignKey2 { get; set; }

        public ICollection<CompositeKeyChild>? Childs { get; set; } = null;
        public ICollection<CompositeKeyChild>? Child2 { get; set; } = null;

    }
}