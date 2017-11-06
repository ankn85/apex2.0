using System.Collections.Generic;

namespace Apex.Data.Paginations
{
    public interface IPagedDataTables<out T> : IEnumerable<T>
    {
        int TotalRecords { get; }

        int TotalRecordsFiltered { get; }
    }
}