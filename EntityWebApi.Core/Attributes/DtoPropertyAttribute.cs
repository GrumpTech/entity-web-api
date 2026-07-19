using EntityWebApi.Core.Enums;

namespace EntityWebApi.Core.Attributes
{
    /// <summary>
    /// Specifies how to handle a target property when creating a dto.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class DtoPropertyAttribute : Attribute
    {
        /// <summary>
        /// Excludes property from dto.
        /// </summary>
        public virtual bool Exclude { get; set; } = false;

        /// <summary>
        /// Makes target property refer to dto with specified suffix if it is a navigation property.
        /// </summary>
        public virtual string? IncludeSuffix { get; set; }

        /// <summary>
        /// Makes target property optional in dto.
        /// </summary>
        public virtual bool Optional { get; set; } = false;

        /// <summary>
        /// Sets access to target property in dto.
        /// </summary>
        public virtual PropertyAccess Access { get; set; } = PropertyAccess.ReadWrite;
    }
}
