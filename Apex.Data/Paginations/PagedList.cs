using System.Collections.Generic;
using System.Linq;
using System;

namespace Apex.Data.Paginations
{
    public sealed class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList(IQueryable<T> source, int page, int size)
        {
            Page = page;
            Size = size;
            int total = source.Count();

            if (total > 0)
            {
                TotalPages = (int)Math.Ceiling(total / (double)size);
                TotalCount = total;
                this.AddRange(page > 1 ?
                    source.Skip((page - 1) * size).Take(size) :
                    source.Take(size));
            }
        }

        public int Page { get; }

        public int Size { get; }

        public int TotalPages { get; }

        public int TotalCount { get; }
    }
}