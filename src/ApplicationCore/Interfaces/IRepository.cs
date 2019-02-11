using ApplicationCore.SharedKernel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IRepository<T> where T : Entity 
    {
        Task<T> GetByIdAsync(long id);
        Task<List<T>> GetAllAsync();
        Task<T> SingleOrDefaultAsync(ISpecification<T> specification);
        Task<T> FirstOrDefaultAsync(ISpecification<T> specification);
        Task<List<T>> ListAsync(ISpecification<T> specification);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
