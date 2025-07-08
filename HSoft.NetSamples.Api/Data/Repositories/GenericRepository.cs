using HSoft.NetSamples.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HSoft.NetSamples.Api.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {

        public readonly MyShopDbContext _context;
        private readonly DbSet<T> _entity;

        public DbSet<T> Entity => _context.Set<T>();
        public GenericRepository(MyShopDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await Entity.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            Entity.Remove(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await Entity.ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where)
        {
            return await Entity.Where(where).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.FindAsync<T>(id);
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
