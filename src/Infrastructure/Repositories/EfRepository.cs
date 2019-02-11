using ApplicationCore.Interfaces;
using ApplicationCore.SharedKernel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly ApplicationDbContext _db;

        public EfRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task<T> SingleOrDefaultAsync(ISpecification<T> specification)
        {
            return await IncludesQuery(specification).Where(specification.Criteria).SingleOrDefaultAsync();
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> specification)
        {
            return await IncludesQuery(specification).Where(specification.Criteria).FirstOrDefaultAsync();
        }

        public async Task<List<T>> ListAsync(ISpecification<T> specification)
        {
            return await IncludesQuery(specification).Where(specification.Criteria).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
        }

        private IQueryable<T> IncludesQuery(ISpecification<T> specification)
        {
            IQueryable<T> queryableResultWithIncludes = specification
                .Includes
                .Aggregate(_db.Set<T>().AsQueryable(), (current, include) => current.Include(include));

            return specification
                .IncludeStrings
                .Aggregate(queryableResultWithIncludes, (current, include) => current.Include(include));
        }
    }
}
