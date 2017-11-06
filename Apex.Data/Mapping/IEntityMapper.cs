using Microsoft.EntityFrameworkCore;

namespace Apex.Data.Mapping
{
    public interface IEntityMapper
    {
        void MapEntities(ModelBuilder modelBuilder);
    }
}
