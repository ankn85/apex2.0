using System.Collections.Generic;

namespace Apex.Data.Paginations
{
    public interface IPagedList<out T> : IEnumerable<T>
    {
        int Page { get; }

        int Size { get; }

        int TotalPages { get; }      

        int TotalCount { get; }
    }
}