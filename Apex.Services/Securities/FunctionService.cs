using Apex.Data.Entities.Securities;
using Apex.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apex.Services.Securities
{
    public sealed class FunctionService : BaseService<Function>, IFunctionService
    {
        public FunctionService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }
        
        public async Task<bool> HasSubFunctions(int id)
        {
            if (id <= 0)
            {
                return false;
            }

            return await QueryNoTracking().AnyAsync(f => f.ParentId == id);
        }

        public async Task<IEnumerable<Function>> GetHierarchicalListAsync(bool? enabled = null)
        {
            var functions = _dbSet.Include(f => f.SubFunctions).AsNoTracking();

            if (enabled.HasValue)
            {
                functions = functions.Where(f => f.Enabled == enabled.Value);
            }

            var hierarchicalFunctions = await functions.OrderBy(f => f.Priority).ToListAsync();

            return hierarchicalFunctions.Where(f => f.ParentId == null || f.ParentId == 0);
        }

        public override async Task<Function> CreateAsync(Function entity)
        {
            await UpdateEnabledDependOnParentAsync(entity);

            return await base.CreateAsync(entity);
        }

        public override async Task UpdateAsync(Function entity)
        {
            await Task.WhenAll(
                UpdateEnabledDependOnParentAsync(entity),
                UpdateDisabledAllSubFunctionssAsync(entity));

            await base.UpdateAsync(entity);
        }

        private async Task<Function> UpdateEnabledDependOnParentAsync(Function entity)
        {
            if (entity.ParentId.HasValue && entity.Enabled)
            {
                Function parent = await FindAsync(entity.ParentId.Value);

                if (parent != null)
                {
                    entity.Enabled = parent.Enabled;
                }
            }

            return entity;
        }

        private async Task UpdateDisabledAllSubFunctionssAsync(Function entity)
        {
            if (!entity.Enabled)
            {
                var hierarchicalFunctions = await _dbSet.Include(f => f.SubFunctions).ToListAsync();

                var subFunctions = hierarchicalFunctions.Where(f => f.ParentId == entity.Id);

                foreach (var item in subFunctions)
                {
                    item.Enabled = false;
                    UpdateDisabledAllSubFunctions(item.SubFunctions);
                }
            }
        }

        private void UpdateDisabledAllSubFunctions(ICollection<Function> functions)
        {
            foreach (var item in functions)
            {
                item.Enabled = false;

                if (item.SubFunctions.Count > 0)
                {
                    UpdateDisabledAllSubFunctions(item.SubFunctions);
                }
            }
        }
    }
}