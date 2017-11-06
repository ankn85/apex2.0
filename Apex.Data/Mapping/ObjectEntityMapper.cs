using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System;

namespace Apex.Data.Mapping
{
    public sealed class ObjectEntityMapper : IEntityMapper
    {
        public void MapEntities(ModelBuilder modelBuilder)
        {
            var type = typeof(IEntityMap);

            var mappings = Assembly.GetAssembly(type).GetTypes()
                .Where(t => t.FullName.Contains(type.Namespace) && t.IsClass && type.IsAssignableFrom(t))
                .Select(t => (IEntityMap)Activator.CreateInstance(t));

            foreach (IEntityMap map in mappings)
            {
                map.Map(modelBuilder);
            }
        }
    }
}
