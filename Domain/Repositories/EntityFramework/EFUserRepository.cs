using Microsoft.EntityFrameworkCore;
using Company.Domain.Entities;
using Company.Domain.Repositories.Abstract;

namespace Company.Domain.Repositories.EntityFramework;

public class EFUserRepository : IRepository<User>
{
    private readonly AppDbContext context;

    public DbSet<User> GetContext => context.DbUsers;

    public EFUserRepository(AppDbContext context)
    {
        this.context = context;
    }

    public IEnumerable<User> Fileter(Func<User, bool> selector)
    {
        foreach(User entity in GetContext)
        {
            if (selector(entity))
            {
                yield return entity;
            }
        }
    }

    public void Remove(Guid id)
    {
        context.DbUsers.Remove(new User { Id = id.ToString("D") }); ;
        context.SaveChanges();
    }

    public void Save(User entity)
    {
        if(entity.Id == default)
        {
            context.Entry(entity).State = EntityState.Added;
        }
        else
        {
            context.Entry(entity).State = EntityState.Modified;
        }
        context.SaveChanges();
    }
}
