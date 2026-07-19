namespace Tests.Shared.Entities
{
    public class ItemWithChilds
    {
        public int Id { get; set; }
        public int Value { get; set; }

        public ICollection<Item> Items { get; set; } = null!;
    }
}