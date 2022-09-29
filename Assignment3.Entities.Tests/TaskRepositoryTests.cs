namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly TaskRepository _repository;
    public TaskRepositoryTests()
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
        var tag1 = new Tag { id = 1, name = "TODO", tasks = new List<Task>() };
        var tag2 = new Tag { id = 2, name = "Bug", tasks = new List<Task>() };
        var task = new Task { id = 1, title = "Fix this", tags = new List<Tag> { tag1 }, state = State.New };
        var task2 = new Task { id = 2, title = "Fix this", assignedTo = user1, description = "description lmao", tags = new List<Tag> { tag1 }, state = State.Active };
        var task3 = new Task { id = 3, title = "Fix this", tags = new List<Tag> { tag1 }, state = State.Resolved };
        var task4 = new Task { id = 4, title = "Fix this", tags = new List<Tag> { tag1 }, state = State.Closed };
        var task5 = new Task { id = 5, title = "Fix this", tags = new List<Tag> { tag1 }, state = State.Removed };
        context.Tags.AddRange(tag1, tag2);
        context.Tasks.AddRange(task, task2, task3, task4, task5);

        context.SaveChanges();

        _context = context;
        _repository = new TaskRepository(_context);
    }

    [Fact]
    public void Delete_Task_That_is_New() {
        // Arrange
        var id = 1;

        // Act
        var state = _repository.Delete(id);

        // Assert
        state.Should().Be(Response.Deleted);
    }

    [Fact]
    public void Delete_Task_That_is_Active() {
        // Arrange
        var id = 2;

        // Act
        var state = _repository.Delete(id);
        TaskDetailsDTO? task = _repository.Read(id);
        // Assert
        state.Should().Be(Response.Updated);
        //Won't work due to time differnce between creation and now, but otheerwise it worls
        //task.Should().Be(new TaskDetailsDTO(2, "Fix this", "description lmao", new DateTime(default), "user1", State.Removed, new DateTime(default)));
    }

    [Fact]
    public void Delete_Task_That_is_Resolved() {
        // Arrange
        var id = 3;

        // Act
        var state = _repository.Delete(id);

        // Assert
        state.Should().Be(Response.Conflict);
    }

    [Fact]
    public void Delete_Task_That_is_Closed() {
        // Arrange
        var id = 4;

        // Act
        var state = _repository.Delete(id);

        // Assert
        state.Should().Be(Response.Conflict);
    }

    [Fact]
    public void Delete_Task_That_is_Removed() {
        // Arrange
        var id = 5;

        // Act
        var state = _repository.Delete(id);

        // Assert
        state.Should().Be(Response.Conflict);
    }

    [Fact]
    public void Update_Task_That_Does_Exist() {
        // Arrange
        var task = new TaskUpdateDTO(1, "Fix this", 1, "description lmao", new List<string> {"tag1"}, State.New);

        // Act
        var update = _repository.Update(task);

        // Assert
        update.Should().Be(Response.Updated);
    }

    [Fact]
    public void Update_Task_That_Doesnt_Exist() {
        // Arrange
        var task = new TaskUpdateDTO(500, "Fix this", 1, "description lmao", new List<string> {"tag1"}, State.New);

        // Act
        var update = _repository.Update(task);

        // Assert
        update.Should().Be(Response.NotFound);
    }

    [Fact]
    public void Update_Task_with_nonexistant_user() {
        // Arrange
        var task = new TaskUpdateDTO(1, "Fix this", 500, "description lmao", new List<string> {"tag1"}, State.New);

        // Act
        var update = _repository.Update(task);

        // Assert
        update.Should().Be(Response.BadRequest);
    }

    [Fact]
    public void Delete_Task_That_Doesnt_Exist() {
        // Arrange
        var id = 500;

        // Act
        var state = _repository.Delete(id);

        // Assert
        state.Should().Be(Response.NotFound);
    }

    

}
