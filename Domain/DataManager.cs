using Company.Domain.Entities;
using Company.Domain.Repositories.Abstract;

namespace Company.Domain;

public class DataManager
{
    public IRepository<User> DbUsers { get; set; }
    public IRepository<Message> DbMessages { get; set; }

    public DataManager(IRepository<Message> messagesRepository, IRepository<User> usersRepository)
    {
        DbUsers = usersRepository;
        DbMessages = messagesRepository;
    }
}
