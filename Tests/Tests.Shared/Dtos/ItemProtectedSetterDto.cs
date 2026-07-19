namespace Tests.Shared.Dtos
{
    public class ItemProtectedSetterDto
    {
        public ItemProtectedSetterDto(int id, int value)
        {
            Id = id;
            Value = value;
        }

        public int Id { get; set; }
        public int Value { get; protected set; }
    }
}