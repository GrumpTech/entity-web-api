using Microsoft.EntityFrameworkCore;

namespace Tests.Shared.Entities
{
    [Keyless]
    public class NoKeyItem
    {
        public int Value { get; set; }
    }
}