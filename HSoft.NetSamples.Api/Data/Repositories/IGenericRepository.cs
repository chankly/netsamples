using HSoft.NetSamples.Api.Domain.Entities;
using System.Linq.Expressions;

namespace HSoft.NetSamples.Api.Data.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task AddAsync(T entity);
        public void Delete(T entity);
        public void Update(T entity);
        public Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where);
        public Task<T> GetByIdAsync(int id);
    }
}
