namespace Tests.Shared.Dtos
{
    public class ItemPrivateSetterNoConstructorDto
    {
        public int Id { get; set; }
        public int Value { get; private set; }
    }
}