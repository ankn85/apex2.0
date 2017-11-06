using Apex.Data.Entities.Logs;
using Microsoft.EntityFrameworkCore;

namespace Apex.Data.Mapping.Logs
{
    public sealed class ActivityLogTypeMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<ActivityLogType>();

            builder.ToTable("ActivityLogTypes", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.SystemKeyword).HasMaxLength(128);

            builder.Property(p => p.Name).HasMaxLength(128);

            builder.HasMany(alt => alt.ActivityLogs)
                .WithOne(al => al.ActivityLogType)
                .HasForeignKey(al => al.ActivityLogTypeId);
        }
    }
}
