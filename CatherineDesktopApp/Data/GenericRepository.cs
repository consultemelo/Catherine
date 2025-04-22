using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CatherineDesktopApp.Shared;

namespace CatherineDesktopApp.Data
{
    public interface IQuerySpecification<T>
    {
        Expression<Func<T, bool>>? Filter { get; }
        List<Expression<Func<T, bool>>> Includes { get; }
        List<string> IncludeStrings { get; }
        QuerySpecification<T> SetFilter(Expression<Func<T, bool>> filter);
        QuerySpecification<T> AddInclude(Expression<Func<T, bool>> include);
        QuerySpecification<T> AddInclude(string include);
    }

    public class QuerySpecification<T> : IQuerySpecification<T>
    {
        public Expression<Func<T, bool>>? Filter { get; private set; }
        public List<Expression<Func<T, bool>>> Includes { get; private set; }
        public List<string> IncludeStrings { get; private set; }

        public QuerySpecification()
        {
            Includes = new List<Expression<Func<T, bool>>>();
            IncludeStrings = new List<string>();
        }

        public QuerySpecification(Expression<Func<T, bool>> filter)
        {
            Filter = filter;
            Includes = new List<Expression<Func<T, bool>>>();
            IncludeStrings = new List<string>();
        }


        public QuerySpecification<T> SetFilter(Expression<Func<T, bool>> filter)
        {
            Filter = filter;
            return this;
        }

        public QuerySpecification<T> AddInclude(Expression<Func<T, bool>> include)
        {
            Includes.Add(include);
            return this;
        }

        public QuerySpecification<T> AddInclude(string include)
        {
            IncludeStrings.Add(include);
            return this;
        }
    }


    public interface IRepository<T>
    {
        Task<IEnumerable<T>> SelectAsync();
        Task<IEnumerable<T>> SelectAsync(IQuerySpecification<T> specification);
        Task<T?> SelectByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(int id, bool hardDelete = false);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> SelectAsync(IQuerySpecification<T> specification)
        {
            IQueryable<T> query = _context.Set<T>();

            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));
            query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            if (specification.Filter != null) query = query.Where(specification.Filter);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> SelectAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> SelectByIdAsync(int id)
        {
            T? result = await _context.Set<T>().FindAsync(id);
            return result;
        }

        public async Task<T> CreateAsync(T entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            if (entity is ISoftDeletableEntity deletableEntity)
            {
                deletableEntity.IsDeleted = false;
            }

            T newEntity = (await _context.Set<T>().AddAsync(entity)).Entity;
            await _context.SaveChangesAsync();
            return newEntity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            T updatedEntity = _context.Set<T>().Update(entity).Entity;
            await _context.SaveChangesAsync();
            return updatedEntity;
        }

        public async Task<T> DeleteAsync(int id, bool hardDelete = false)
        {
            T? entity = await SelectByIdAsync(id);

            if (entity == null) throw new ArgumentException(nameof(entity));

            if (entity is ISoftDeletableEntity deletableEntity && !hardDelete)
            {
                deletableEntity.IsDeleted = true;
                await UpdateAsync(entity);
                return entity;
            }
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
