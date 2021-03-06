using Apex.Data.Entities.Logs;
using Microsoft.EntityFrameworkCore;

namespace Apex.Data.Mapping.Logs
{
    public sealed class LogMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Log>();

            builder.ToTable("Logs", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.Application).HasMaxLength(256);

            builder.Property(p => p.Level).HasMaxLength(256);

            builder.Property(p => p.Logger).HasMaxLength(256);
        }
    }
}
