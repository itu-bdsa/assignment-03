

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
        var tag1 = new Tag { id = 1, name = "TODO", tasks = new List<Task>() };
        var tag2 = new Tag { id = 2, name = "Bug", tasks = new List<Task>() };
        var task = new Task { id = 1, title = "Fix this", tags = new List<Tag> { tag1 }, state = State.New };
        context.Tags.AddRange(tag1, tag2);
        context.Tasks.Add(task);

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
        created.Should().Be(new TagDTO(3, "Program").Id);
    }


    public void Dispose()
    {
        _context.Dispose();
    }
}
