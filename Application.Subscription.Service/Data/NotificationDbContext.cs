using Application.Subscription.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Subscription.Service.Data
{
    public class NotificationDbContext(DbContextOptions<NotificationDbContext> options) : DbContext(options)
    {
        public DbSet<SubscriptionEntity> Subscriptions => Set<SubscriptionEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubscriptionEntity>(entity =>
            {
                entity.ToTable("Subscriptions");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.EventType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.WebhookUrl)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(x => x.CreatedAtUtc)
                    .IsRequired();
            });
        }
    }
}
