using Microsoft.EntityFrameworkCore;
using Company.Domain.Entities;
using Company.Domain.Repositories.Abstract;

namespace Company.Domain.Repositories.EntityFramework;

public class EFMessageRepository : IRepository<Message>
{
    private readonly AppDbContext context;

    public DbSet<Message> GetContext => context.DbMessages;

    public EFMessageRepository(AppDbContext context)
    {
        this.context = context;
    }

    public IEnumerable<Message> Fileter(Func<Message, bool> selector)
    {
        foreach (Message entity in GetContext)
        {
            if (selector(entity))
            {
                yield return entity;
            }
        }
    }

    public void Remove(Guid id)
    {
        context.DbMessages.Remove(new Message { Id = id });
        context.SaveChanges();
    }

    public void Save(Message entity)
    {
        if (entity.Id == default)
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
