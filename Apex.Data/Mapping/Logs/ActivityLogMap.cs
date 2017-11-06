using Apex.Data.Entities.Logs;
using Microsoft.EntityFrameworkCore;

namespace Apex.Data.Mapping.Logs
{
    public sealed class ActivityLogMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<ActivityLog>();

            builder.ToTable("ActivityLogs", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.ObjectFullName).IsRequired().HasMaxLength(128);

            builder.Property(p => p.OldValue);

            builder.Property(p => p.NewValue).IsRequired();

            builder.Property(p => p.IP).IsRequired().HasMaxLength(64);

            builder.HasOne(al => al.ApplicationUser)
                .WithMany(u => u.ActivityLogs)
                .HasForeignKey(al => al.ApplicationUserId);
        }
    }
}
