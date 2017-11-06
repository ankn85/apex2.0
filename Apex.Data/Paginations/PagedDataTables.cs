using System.Collections.Generic;
using System.Linq;

namespace Apex.Data.Paginations
{
    public sealed class PagedDataTables<T> : List<T>, IPagedDataTables<T>
    {
        private PagedDataTables(IEnumerable<T> source, int totalRecords, int totalRecordsFiltered)
        {
            TotalRecords = totalRecords;
            TotalRecordsFiltered = totalRecordsFiltered;

            if (TotalRecordsFiltered > 0)
            {
                AddRange(source);
            }
        }

		public int TotalRecords { get; }

        public int TotalRecordsFiltered { get; }

        public static IPagedDataTables<T> Empty()
        {
            return new PagedDataTables<T>(Enumerable.Empty<T>(), 0, 0);
        }

        public static IPagedDataTables<T> Empty(int totalRecords)
        {
            return new PagedDataTables<T>(Enumerable.Empty<T>(), totalRecords, 0);
        }

        public static IPagedDataTables<T> Create(IEnumerable<T> source, int totalRecords, int totalRecordsFiltered)
        {
            return new PagedDataTables<T>(source, totalRecords, totalRecordsFiltered);
        }
    }
}