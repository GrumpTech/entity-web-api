namespace Tests.Shared.Dtos
{
    public class ItemPrivateSetterDto
    {
        public ItemPrivateSetterDto(int id, int value)
        {
            Id = id;
            Value = value;
        }

        public int Id { get; set; }
        public int Value { get; private set; }
    }
}