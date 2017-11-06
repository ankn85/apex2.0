using Apex.Data.Entities.Logs;
using Apex.Data.Enums;
using Apex.Data.Paginations;
using System.Threading.Tasks;
using System;

namespace Apex.Services.Logs
{
    public interface ILogService : IService<Log>
    {
        Task<IPagedDataTables<Log>> GetPagedListAsync(
            DateTime fromDate, 
            DateTime toDate,
            string[] levels,
            string sortColumnName,
            SortDirection sortDirection,
            int page, 
            int size);
	}
}
