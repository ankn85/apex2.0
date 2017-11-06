using Apex.Data.Entities.Logs;
using Apex.Data.Enums;
using Apex.Data.Paginations;
using System.Threading.Tasks;
using System;

namespace Apex.Services.Logs
{
    public interface IActivityLogService : IService<ActivityLog>
    {
        Task<IPagedDataTables<ActivityLog>> GetPagedListAsync(
            DateTime fromDate,
            DateTime toDate,
            string userName,
            string ip,
            string sortColumnName,
            SortDirection sortDirection,
            int page,
            int size);
            
        Task CreateAsync(string systemKeyword, ActivityLog entity);
    }
}