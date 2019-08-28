using Microsoft.EntityFrameworkCore;
using Stores.Domain.SeedWork;
using Stores.Infrastructure.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stores.Infrastructure.Repositories
{
    public interface IRepository<TEntity>
        where TEntity: Entity
    {
        IQueryable<TEntity> Find();

        Task Create(TEntity entity);

        Task Update(long id, TEntity entity);

        Task Delete(long id);
        
        Task<TEntity> GetById(long id);
    }

    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        public readonly StorePackageContext Context;

        public Repository (StorePackageContext packageContext)
        {
            Context = packageContext;
        }

        public async Task Create(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var entity = await GetById(id);

            Context.Set<TEntity>().Remove(entity);

            await Context.SaveChangesAsync();
        }

        public async Task<TEntity> GetById(long id)
        {
            return await Find()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public IQueryable<TEntity> Find()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }

        public async Task Update(long id, TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            await Context.SaveChangesAsync();
        }
    }
}
