using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Company.Domain.Entities;

namespace Company.Domain;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> DbUsers { get; set; }
    public DbSet<Message> DbMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().HasData(new User
        {
            Id = "3b63772e-4ffa-49ed-a20f-e7685b9967d7",
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "skliardev@gmail.com",
            Published = DateTime.UtcNow,
            Status = UserStatus.OFFLINE,
            PasswordHash = new PasswordHasher<User>().HashPassword(null, "admin"),
        }); 

        builder.Entity<User>().HasData(new User
        {
            Id = "3b63772e-4ffa-49ed-a20f-e7685b9967d1",
            UserName = "publisher",
            NormalizedUserName = "PUBLISHER",
            Email = "publisher@gmail.com",
            Published = DateTime.UtcNow,
            Status = UserStatus.OFFLINE,
            PasswordHash = new PasswordHasher<User>().HashPassword(null, "publisher"),
        });
    }
}
