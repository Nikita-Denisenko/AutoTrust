using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private DbSet<T> Set => _context.Set<T>();

        public Repository(AppDbContext context) => _context = context;
        
        public async Task AddAsync(T entity, CancellationToken cancellationToken) 
            => await Set.AddAsync(entity, cancellationToken);

        public void Delete(T entity) => Set.Remove(entity);

        public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await GetByIdAsync(id, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException($"Entity by Id {id} was not found!");

            Delete(entity);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken) 
            => await Set.FindAsync(id, cancellationToken);

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
            => await _context.SaveChangesAsync(cancellationToken);

        public IQueryable<T> GetQuery() => Set;
    }
}
