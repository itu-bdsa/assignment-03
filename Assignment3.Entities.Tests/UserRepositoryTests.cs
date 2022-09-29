namespace Assignment3.Entities.Tests;

public class UserRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        //Turn off foreign key checks
        var command = connection.CreateCommand();
        command.CommandText = "PRAGMA foreign_keys = OFF;";
        command.ExecuteNonQuery();

        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();

        var user1 = new User { id = 1, name = "user1", email = "user1"};
        var user2 = new User { id = 2, name = "user2", email = "user2"};
        var user3 = new User { id = 3, name = "user3", email = "user3"};
        var tag1 = new Tag { id = 1, name = "Tag1", tasks = new List<Task>() };
        var tag2 = new Tag { id = 2, name = "Tag2", tasks = new List<Task>() };
        var task1 = new Task { id = 1, title = "Fix this", assignedTo = user1, tags = new List<Tag> { tag1 }, state = State.New };
        var task2 = new Task { id = 2, title = "Empty", assignedTo = user1, tags = new List<Tag> { tag2 }, state = State.New };
        user1.tasks = new List<Task> { task1, task2 };
        user2.tasks = new List<Task> { };
        user3.tasks = new List<Task> { };

        context.Tasks.AddRange(task1, task2);
        context.Users.AddRange(user1, user2, user3);

        context.SaveChanges();

        _context = context;
        _repository = new UserRepository(context);

    }

    [Fact]
    public void Create_user_returns_success() {
        // Arrange
        var user = new UserCreateDTO("newuser", "newuser");

        // Act
        var (state, id) = _repository.Create(user);

        // Assert
        state.Should().Be(Response.Created);
    }

    [Fact]
    public void Delete_user_used_in_task_returns_user() {
        // Arrange
        var id = 1;

        // Act
        var state = _repository.Delete(id);
        UserDTO? user = _repository.Read(id);

        // Assert
        state.Should().Be(Response.Conflict);
        user.Should().Be(new UserDTO(1, "user1", "user1"));
    }

    [Fact]
    public void Force_delete_user_used_in_task_returns_success() {
        // Arrange
        var id = 1;

        // Act
        var state = _repository.Delete(id, force: true);
        UserDTO? user = _repository.Read(id);

        // Assert
        state.Should().Be(Response.Deleted);
        user.Should().Be(null);
    }

    [Fact]
    public void Delete_user_returns_success() {
        // Arrange
        var id = 2;

        // Act
        var state = _repository.Delete(id);
        UserDTO? user = _repository.Read(id);

        // Assert
        state.Should().Be(Response.Deleted);
        user.Should().Be(null);
    }

    [Fact]
    public void Create_user_that_already_exist_returns_conflict() {
        // Arrange
        var user = new UserCreateDTO("user3", "user3");

        // Act
        var (state, id) = _repository.Create(user);

        // Assert
        state.Should().Be(Response.Conflict);
    }

    [Fact]
    public void Update_User_That_Does_Exist() {
        // Arrange
        var user = new UserUpdateDTO(1, "user3", "user3");

        // Act
        var update = _repository.Update(user);

        // Assert
        update.Should().Be(Response.Updated);
    }

    [Fact]
    public void Update_User_That_Doesnt_Exist() {
        // Arrange
        var user = new UserUpdateDTO(500, "user3", "user3");

        // Act
        var update = _repository.Update(user);

        // Assert
        update.Should().Be(Response.NotFound);
    }

    [Fact]
    public void Delete_User_That_Doesnt_Exist() {
        // Arrange
        var id = 500;

        // Act
        var state = _repository.Delete(id);

        // Assert
        state.Should().Be(Response.NotFound);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

}
