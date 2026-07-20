using EntityWebApi.Core.Attributes;

namespace Infrastructure.Attributes
{
    public class PatchDtoPropertyAttribute : DtoPropertyAttribute
    {
        public override bool Optional { get; set; } = true;
    }
}
