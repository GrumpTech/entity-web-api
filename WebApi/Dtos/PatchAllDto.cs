namespace WebApi.Dtos
{
    public class PatchAllDto<TKey, TPatchDto>
    {
        public TKey[] Keys { get; set; } = null!;
        public TPatchDto PatchDto { get; set; } = default!;
    }
}
