using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Assignment3.Core;

namespace Assignment3.Entities.Tests;

public class UserRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly UserRepository _repository;
    public UserRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        context.Users.AddRange(new User("Bob", "bob@gmail.com") { Id = 1 }, new User("Michael", "Michael@gmail.com") { Id = 2 });
        context.WorkItems.Add(new WorkItem { Id = 1, Title = "Bob Project"});
        context.SaveChanges();

        _context = context;
        _repository = new UserRepository(_context);
    }

    public void Dispose() {
        _context.Dispose();
    }
}
