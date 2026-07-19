namespace Tests.Shared.Dtos
{
    public class ItemWithChildsDto
    {
        public int Id { get; set; }
        public int Value { get; set; }

        public ItemDto[] Items { get; set; } = null!;
    }
}