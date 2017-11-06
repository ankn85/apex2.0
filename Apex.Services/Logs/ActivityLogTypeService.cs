using Apex.Core.Caching;
using Apex.Data.Entities.Logs;
using Apex.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apex.Services.Logs
{
    public sealed class ActivityLogTypeService : BaseService<ActivityLogType>, IActivityLogTypeService
    {
        private const string EnabledActivityLogTypesKey = "cache.enabledactivitylogtypes";

        private readonly IMemoryCacheService _memoryCacheService;

        public ActivityLogTypeService(
            ObjectDbContext dbContext,
            IMemoryCacheService memoryCacheService) : base(dbContext)
        {
            _memoryCacheService = memoryCacheService;
        }

        public IDictionary<string, int> GetEnabledIdList()
        {
            return _memoryCacheService.GetSlidingExpiration(
                EnabledActivityLogTypesKey,
                () =>
                {
                    return QueryNoTracking()
                        .Where(alt => alt.Enabled)
                        .ToDictionary(alt => alt.SystemKeyword, alt => alt.Id);
                });
        }

        public override async Task UpdateAsync(ActivityLogType entity)
        {
            await base.UpdateAsync(entity);

            _memoryCacheService.Remove(EnabledActivityLogTypesKey);
        }
    }
}
