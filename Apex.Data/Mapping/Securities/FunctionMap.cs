using Apex.Data.Entities.Securities;
using Microsoft.EntityFrameworkCore;

namespace Apex.Data.Mapping.Securities
{
    public sealed class FunctionMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Function>();

            builder.ToTable("Functions", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(128);
        }
    }
}
