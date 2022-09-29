namespace Assignment3.Entities.Tests;


public class TagRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly TagRepository _repository;

    public TagRepositoryTests()
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
        var tag1 = new Tag { id = 1, name = "Tag1", tasks = new List<Task>() };
        var tag2 = new Tag { id = 2, name = "Tag2", tasks = new List<Task>() };
        var tag3 = new Tag { id = 3, name = "Tag3", tasks = new List<Task>() };
        var tag4 = new Tag { id = 4, name = "Tag4", tasks = new List<Task>() };
        var task1 = new Task { id = 1, title = "Fix this", tags = new List<Tag> { tag1, tag2 }, state = State.New };
        var task2 = new Task { id = 2, title = "Empty", tags = new List<Tag> { }, state = State.New };
        context.Tags.AddRange(tag1, tag2, tag3, tag4);
        context.Tasks.AddRange(task1, task2);

        context.SaveChanges();

        _context = context;
        _repository = new TagRepository(_context);
    }

    [Fact]
    public void Given_new_tag_returns_tag()
    {
        // Arrange
        var (response, created) = _repository.Create(new TagCreateDTO("Program"));

        // Act
        response.Should().Be(Created);

        // Assert
        created.Should().Be(new TagDTO(5, "Program").Id);
    }

    [Fact]
    public void Delete_tag_used_in_task_returns_tag() {
        // Arrange
        var id = 1;

        // Act
        var state = _repository.Delete(id);
        TagDTO? tag = _repository.Read(id);

        // Assert
        state.Should().Be(Response.Conflict);
        tag.Should().Be(new TagDTO(1, "Tag1"));
    }

    [Fact]
    public void Force_delete_tag_used_in_task_returns_success() {
        // Arrange
        var id = 2;

        // Act
        var state = _repository.Delete(id, force: true);
        TagDTO? tag = _repository.Read(id);

        // Assert
        state.Should().Be(Response.Deleted);
        tag.Should().Be(null);
    }

    [Fact]
    public void Delete_tag_returns_success() {
        // Arrange
        var id = 3;

        // Act
        var state = _repository.Delete(id);
        TagDTO? tag = _repository.Read(id);

        // Assert
        state.Should().Be(Response.Deleted);
        tag.Should().Be(null);
    }

    [Fact]
    public void Create_tag_that_already_exist_returns_conflict() {
        // Arrange
        var tagDTO = new TagCreateDTO("Tag4");

        // Act
        var (state, id) = _repository.Create(tagDTO);

        // Assert
        state.Should().Be(Response.Conflict);
    }

    [Fact]
    public void Update_Tag_That_Does_Exist() {
        // Arrange
        var tag = new TagUpdateDTO(1, "Tag4");

        // Act
        var update = _repository.Update(tag);

        // Assert
        update.Should().Be(Response.Updated);
    }

    [Fact]
    public void Update_Tag_That_Doesnt_Exist() {
        // Arrange
        var tag = new TagUpdateDTO(500, "Tag4");

        // Act
        var update = _repository.Update(tag);

        // Assert
        update.Should().Be(Response.NotFound);
    }

    [Fact]
    public void Delete_tag_That_Doesnt_Exist() {
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
