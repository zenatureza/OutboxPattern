using Microsoft.EntityFrameworkCore;
using OutboxPattern.Entities;
using OutboxPattern.Persistence.Outbox;

namespace OutboxPattern.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Activation> Activations { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.ToTable("OutboxMessages");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
