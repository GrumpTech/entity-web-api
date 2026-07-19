namespace EntityWebApi.Dtos.Internal.Enums
{
    [Flags]
    public enum EntityPropertyTypes
    {
        None = 0,
        All = PrimaryKey | Regular | ForeignKeys | Navigations | SkipNavigations,
        PrimaryKey = 1,
        Regular = 2,
        ForeignKeys = 4,
        Navigations = 8,
        SkipNavigations = 16
    }
}
