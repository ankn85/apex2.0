using Apex.Data.Entities;
using Apex.Data.Enums;
using Apex.Data.Paginations;
using Apex.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System;

namespace Apex.Services
{
    public abstract class BaseService<T> : IService<T> where T : BaseEntity
    {
        protected readonly ObjectDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public BaseService(ObjectDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        #region Query Functions

        public virtual async Task<T> FindAsync(int id)
        {
            return id <= 0 ?
                null :
                await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return Enumerable.Empty<T>();
            }

            return await _dbSet.Where(e => ids.Contains(e.Id)).ToListAsync();
        }

        public virtual IQueryable<T> QueryNoTracking()
        {
            return _dbSet.AsNoTracking();
        }

        public virtual IPagedList<T> GetPagedList(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int page, int size)
        {
            return GetPagedList(orderBy(QueryNoTracking()), page, size);
        }

        public virtual async Task<IList<T>> GetListAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var query = orderBy == null ? QueryNoTracking() : orderBy(QueryNoTracking());

            return await query.ToListAsync();
        }

        public virtual async Task<IPagedDataTables<T>> GetPagedListAsync(
            string sortColumnName,
            SortDirection sortDirection,
            int page = 0,
            int size = 0)
        {
            var query = QueryNoTracking();
            int totalRecords = await query.CountAsync();

            if (totalRecords == 0)
            {
                return PagedDataTables<T>.Empty();
            }

            int totalRecordsFiltered = totalRecords;
            query = OrderBy(query, sortColumnName, sortDirection);

            if (page > 0 && size > 0)
            {
                query = query.Skip(page).Take(size);
            }

            return PagedDataTables<T>.Create(query, totalRecords, totalRecordsFiltered);
        }

        protected static IPagedList<T> GetPagedList(IQueryable<T> source, int page, int size)
        {
            return new PagedList<T>(source, page, size);
        }

        protected static IQueryable<T> OrderBy(IQueryable<T> source, string propertyName, SortDirection sortDirection)
        {
            if (!source.Any() || string.IsNullOrWhiteSpace(propertyName))
            {
                return source;
            }

            PropertyInfo property = GetProperty(propertyName);

            if (property == null)
            {
                return source;
            }

            var sortedSource = sortDirection == SortDirection.Ascending ?
                source.OrderBy(property.GetValue) :
                source.OrderByDescending(property.GetValue);

            return sortedSource.AsQueryable();
        }

        private static PropertyInfo GetProperty(string propertyName)
        {
            return typeof(T).GetProperties()
                .FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region Command Functions

        public virtual async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        #endregion
    }
}