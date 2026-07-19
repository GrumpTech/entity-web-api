namespace EntityWebApi.Core.Enums
{
    [Flags]
    public enum PropertyAccess
    {
        None = 0,
        Read = 1,
        Write = 2,
        ReadWrite = 3,
    }
}
