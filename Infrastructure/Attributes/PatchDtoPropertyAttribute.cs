using EntityWebApi.Core.Attributes;

namespace Infrastructure.Attributes
{
    public class PatchDtoProperty : DtoPropertyAttribute
    {
        public override bool Optional { get; set; } = true;
    }
}
