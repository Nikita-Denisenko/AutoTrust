namespace AutoTrust.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task AddAsync(T entity, CancellationToken cancellationToken);
        public void Delete(T entity);
        public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task SaveChangesAsync(CancellationToken cancellationToken);
        public Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
        public IQueryable<T> GetQuery();
    }
}
