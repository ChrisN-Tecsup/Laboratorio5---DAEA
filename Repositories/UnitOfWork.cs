using Lab5_ChristianMamani.Models;
using System.Collections;

namespace Lab5_ChristianMamani.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Lab5DbContext _context;
        private Hashtable? _repositories;

        public UnitOfWork(Lab5DbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity).Name;

            if (_repositories!.ContainsKey(type))
            {
                return (IGenericRepository<TEntity>)_repositories[type]!;
            }

            var repositoryType = typeof(GenericRepository<>);
            var repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(typeof(TEntity)), _context);

            if (repositoryInstance != null)
            {
                _repositories.Add(type, repositoryInstance);
                return (IGenericRepository<TEntity>)repositoryInstance;
            }

            throw new Exception($"Could not create repository instance for type {type}");
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}