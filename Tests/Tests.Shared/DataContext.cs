using Microsoft.EntityFrameworkCore;
using Tests.Shared.Entities;

namespace Tests.Shared
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<Item> Items { get; set; }
        public DbSet<Item2> Items2 { get; set; }
        public DbSet<CompositeKeyItem> CompositeKeyItems { get; set; }
        public DbSet<NoKeyItem> NoKeyItems { get; set; }
        public DbSet<ItemWithChilds> ItemsWithChilds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompositeKeyItem>().HasKey(c => new { c.Key, c.Key2 });
        }
    }
}