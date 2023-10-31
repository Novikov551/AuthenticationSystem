using AuthenticationSystem.Domain;
using AuthenticationSystem.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationSystem.Infrastructure.Repositories;

internal class BaseEfRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _entities;

    public BaseEfRepository(DbSet<T> entities)
    {
        _entities = entities;
    }

    public async ValueTask<T?> FindAsync(Guid id)
    {
        return await _entities.FindAsync(id);
    }

    public IQueryable<T> Query => _entities;

    public T Create(T entity)
    {
        _entities.Add(entity);
        PrepareToCreate(entity);

        return entity;
    }

    public IEnumerable<T> Create(IEnumerable<T> entities)
    {
        var list = entities.ToList();
        foreach (var entity in list)
        {
            PrepareToCreate(entity);
        }

        _entities.AddRange(list);

        return list;
    }

    public T Update(T entity, bool autoVersionUpdate = true)
    {
        if (autoVersionUpdate)
        {
            PrepareToUpdate(entity);
        }

        _entities.Update(entity);

        return entity;
    }

    public IEnumerable<T> Update(IEnumerable<T> entities, bool autoVersionUpdate = true)
    {
        var list = entities.ToList();

        if (autoVersionUpdate)
        {
            foreach (var entity in list)
            {
                PrepareToUpdate(entity);
            }
        }

        _entities.UpdateRange(list);

        return list;
    }

    public void Remove(T entity)
    {
        _entities.Remove(entity);
    }

    public void Remove(IEnumerable<T> entities)
    {
        _entities.RemoveRange(entities);
    }

    public IQueryable<T> FromSqlRaw(string sql)
    {
        return _entities.FromSqlRaw(sql);
    }

    private static void PrepareToCreate(T entity)
    {
        if (entity.Version == Guid.Empty)
        {
            entity.Version = Guid.NewGuid();
        }

        entity.CreatedDate = DateTime.UtcNow;
    }

    private static void PrepareToUpdate(T entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;

        if (entity.Version == Guid.Empty)
        {
            entity.Version = Guid.NewGuid();
        }
    }
}