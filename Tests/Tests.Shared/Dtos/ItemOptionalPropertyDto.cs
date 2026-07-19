using EntityWebApi.Core.Attributes;

namespace Tests.Shared.Dtos
{
    public class ItemOptionalPropertyDto
    {
        public readonly bool HasValue;
        public int Id { get; set; }
        [HasValueField(nameof(HasValue))]
        public int Value { get; set; }

        public ItemOptionalPropertyDto(int id)
        {
            Id = id;
            HasValue = false;
        }
        public ItemOptionalPropertyDto(int id, int value)
        {
            Id = id;
            Value = value;
            HasValue = true;
        }
    }
}