using Apex.Core.Extensions;
using Apex.Data.Entities.Logs;
using Apex.Data.Enums;
using Apex.Data.Paginations;
using Apex.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Apex.Services.Logs
{
    public sealed class ActivityLogService : BaseService<ActivityLog>, IActivityLogService
    {
        private readonly IActivityLogTypeService _activityLogTypeService;

        public ActivityLogService(
            ObjectDbContext dbContext,
            IActivityLogTypeService activityLogTypeService) : base(dbContext)
        {
            _activityLogTypeService = activityLogTypeService;
        }

        public async Task<IPagedDataTables<ActivityLog>> GetPagedListAsync(
            DateTime fromDate,
            DateTime toDate,
            string userName,
            string ip,
            string sortColumnName,
            SortDirection sortDirection,
            int page,
            int size)
        {
            int totalRecords = await QueryNoTracking().CountAsync();

            if (totalRecords == 0)
            {
                return PagedDataTables<ActivityLog>.Empty();
            }

            var query = _dbSet
                .Include(al => al.ActivityLogType)
                .Include(al => al.ApplicationUser)
                .AsNoTracking();

            if (fromDate != DateTime.MinValue &&
                toDate != DateTime.MinValue &&
                fromDate <= toDate)
            {
                DateTime startDate = fromDate.StartOfDay();
                DateTime endDate = toDate.EndOfDay();
                
                query = query.Where(al => startDate <= al.CreatedOn && al.CreatedOn <= endDate);
            }

            if (!string.IsNullOrWhiteSpace(userName))
            {
                query = query.Where(al => al.ApplicationUser.UserName.StartsWith(userName));
            }

            if (!string.IsNullOrWhiteSpace(ip))
            {
                query = query.Where(al => al.IP.StartsWith(ip));
            }

            int totalRecordsFiltered = await query.CountAsync();

            if (totalRecordsFiltered == 0)
            {
                return PagedDataTables<ActivityLog>.Empty(totalRecords);
            }

            query = OrderBy(query, sortColumnName, sortDirection).Skip(page).Take(size);

            return PagedDataTables<ActivityLog>.Create(query, totalRecords, totalRecordsFiltered);
        }

        public async Task CreateAsync(string systemKeyword, ActivityLog entity)
        {
            var activityLogTypeId = GetActivityLogTypeId(systemKeyword);

            if (activityLogTypeId.HasValue)
            {
                entity.ActivityLogTypeId = activityLogTypeId.Value;

                await base.CreateAsync(entity);
            }
        }

        private int? GetActivityLogTypeId(string systemKeyword)
        {
            int activityLogTypeId;
            var enabledList = _activityLogTypeService.GetEnabledIdList();

            if (enabledList == null || !enabledList.TryGetValue(systemKeyword, out activityLogTypeId))
            {
                return null;
            }

            return activityLogTypeId;
        }
    }
}
