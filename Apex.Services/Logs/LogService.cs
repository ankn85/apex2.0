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
    public sealed class LogService : BaseService<Log>, ILogService
    {
        public LogService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IPagedDataTables<Log>> GetPagedListAsync(
            DateTime fromDate,
            DateTime toDate,
            string[] levels,
            string sortColumnName,
            SortDirection sortDirection,
            int page,
            int size)
        {
            var query = QueryNoTracking();

            int totalRecords = await query.CountAsync();

            if (totalRecords == 0)
            {
                return PagedDataTables<Log>.Empty();
            }

            if (fromDate != DateTime.MinValue &&
                toDate != DateTime.MinValue &&
                fromDate <= toDate)
            {
                DateTime startDate = fromDate.StartOfDay();
                DateTime endDate = toDate.EndOfDay();

                query = query.Where(l => startDate <= l.Logged && l.Logged <= endDate);
            }

            if (levels != null && levels.Length > 0)
            {
                query = query.Where(l => levels.Contains(l.Level));
            }

            int totalRecordsFiltered = await query.CountAsync();

            if (totalRecordsFiltered == 0)
            {
                return PagedDataTables<Log>.Empty(totalRecords);
            }

            query = OrderBy(query, sortColumnName, sortDirection).Skip(page).Take(size);
            
            return PagedDataTables<Log>.Create(query, totalRecords, totalRecordsFiltered);
        }
    }
}
