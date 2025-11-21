using Microsoft.EntityFrameworkCore;
using Saki.IDbBase.Entities;

namespace Saki.EFBase
{
    public abstract class BaseEfCoreDbClient<TContext> : IDbClient
            where TContext : DbContext
    {
        protected readonly TContext _db;

        protected BaseEfCoreDbClient(TContext db)
        {
            _db = db;
        }

        public virtual async Task<int> ExecuteAsync(string sql, params object[] parameters)
        {
            return await _db.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public virtual async Task<IEnumerable<T>> QueryAsync<T>(string sql, params object[] parameters)
            where T : class, IEntity
        {
            return await _db.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
        }



        public virtual async Task EnsureCreatedAsync()
        {
            await _db.Database.EnsureCreatedAsync();
        }
    }
}
