using Microsoft.EntityFrameworkCore;

namespace Tests.Shared.Dtos
{
    [Keyless]
    public class NoKeyItemDto
    {
        public int Value { get; set; }
    }
}