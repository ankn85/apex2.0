using Apex.Data.Entities;
using Apex.Data.Enums;
using Apex.Data.Paginations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Apex.Services
{
    public interface IService<T> where T : BaseEntity
    {
        Task<T> FindAsync(int id);

        Task<IEnumerable<T>> FindAsync(int[] ids);

        IQueryable<T> QueryNoTracking();

        IPagedList<T> GetPagedList(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int page, int size);

        Task<IList<T>> GetListAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        Task<IPagedDataTables<T>> GetPagedListAsync(
            string sortColumnName,
            SortDirection sortDirection,
            int page = 0,
            int size = 0);

        Task<T> CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task DeleteAsync(IEnumerable<T> entities);

        Task<int> SaveChangesAsync();
    }
}
