using Apex.Data.Entities.Blobs;
using Microsoft.EntityFrameworkCore;

namespace Apex.Data.Mapping.Blobs
{
    public sealed class BlobMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Blob>();

            builder.ToTable("Blobs", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.FileName).IsRequired().HasMaxLength(128);

            builder.Property(p => p.FileExtension).HasMaxLength(16);

            builder.Property(p => p.ContentType).HasMaxLength(64);

            builder.Property(p => p.Path).IsRequired().HasMaxLength(256);
        }
    }
}
