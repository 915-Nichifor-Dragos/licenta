namespace HotelManagement.DataAccess.IRepository;

public interface IAbstractRepository<T>
{
    IQueryable<T> GetAll();
    Task<T> GetById(Guid id);
    Task SaveChanges();
    Task Add(T entity);
    Task<T> Update(T entity);
    Task Delete(T entityToDelete);
}