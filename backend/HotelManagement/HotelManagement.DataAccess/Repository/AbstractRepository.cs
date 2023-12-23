using HotelManagement.DataAccess.Contexts;
using HotelManagement.DataAccess.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.DataAccess.IRepositories;

public class AbstractRepository<T> : IAbstractRepository<T> where T : class
{
    protected HotelManagementContext _dbContext;

    protected DbSet<T> _dbSet;

    public AbstractRepository(HotelManagementContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    public IQueryable<T> GetAll()
    {
        return _dbSet;
    }

    public virtual async Task<T> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task SaveChanges()
    {
        await _dbContext.SaveChangesAsync(true);
    }

    public virtual async Task Add(T entity)
    {
        _dbSet.Add(entity);
        await SaveChanges();
    }

    public virtual async Task<T> Update(T entity)
    {
        var updatedEntity = _dbSet.Update(entity).Entity;
        await SaveChanges();

        return updatedEntity;
    }

    public virtual async Task Delete(T entityToDelete)
    {
        if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }

        _dbSet.Remove(entityToDelete);
        await SaveChanges();
    }
}