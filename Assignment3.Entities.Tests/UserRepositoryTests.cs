using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Assignment3.Core;
using Microsoft.EntityFrameworkCore.Sqlite;

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
        var userForWI = new User("BobMichael", "bobMichael@gmail.com" ) {Id = 3};
        context.Users.AddRange(new User("Bob", "bob@gmail.com") { Id = 1 }, new User("Michael", "Michael@gmail.com") { Id = 2 }, userForWI);
        context.WorkItems.Add(new WorkItem { Id = 1, Title = "Bob Project", Description = "bla bla bla", AssignedTo = userForWI});
        context.SaveChanges();

        _context = context;
        _repository = new UserRepository(_context);
    }

[Fact]
public void createUserreturnsCreatedWithUser() {
//Arrange
var (response, userId) = _repository.Create(new UserCreateDTO("Clara", "Clara@gmail.com"));
var user = new UserDTO(4, "Clara", "Clara@gmail.com");
//Act
//Assert
response.Should().Be(Response.Created);

userId.Should().Be(user.Id);
}

[Fact]
public void readGivenIdReturnsUser() {
//Arrange
var actual = _repository.Read(1);
var expected = new UserDTO(1, "Bob", "bob@gmail.com");
//Act
//Assert
actual.Should().Be(expected);
}

[Fact]
public void readAllReturnsAllUsers() {
//Arrange
var actual = _repository.ReadAll().Count();
//Act
//Assert
actual.Should().Be(3);
}

[Fact]
public void updateReturnsUpdatedWhenGivenExistingUser() {
//Arrange
var user = new UserUpdateDTO(1, "Bob,", "BobsnyeMail@gmail.com");
//Act
var Response = _repository.Update(user);
var foundUser = _repository.Read(1);
var updatedEmail = foundUser.Email;
//Assert
Response.Should().Be(Response.Updated);
updatedEmail.Should().Be("BobsnyeMail@gmail.com");
}

[Fact]
public void DeleteReturnsDeletedWhenExistingUserIdGiven() {
//Arrange
//Act
var Response = _repository.Delete(1);
var ReadResponse = _repository.Read(1);
//Assert
Response.Should().Be(Response.Deleted);
ReadResponse.Should().Be(null);
}



    public void Dispose() {
        _context.Dispose();
    }
}
