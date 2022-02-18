using Microsoft.EntityFrameworkCore;

namespace Company.Domain.Repositories.Abstract;

public interface IRepository<T> where T : class
{
    DbSet<T> GetContext{ get; }
    public IEnumerable<T> Fileter(Func<T, bool> selector);
    public void Remove(Guid id);
    public void Save(T entity);
}
