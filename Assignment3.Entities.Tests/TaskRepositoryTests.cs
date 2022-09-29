namespace Assignment3.Entities.Tests;
using Assignment3.Core;
using Assignment3;

public class TaskRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly TaskRepository _repo;

    public TaskRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();

        context.Tasks.Add(new Task("Lave aftensmad", Core.State.New));
        context.Tasks.Add(new Task("Spise dessert", Core.State.Active));
        context.SaveChanges();

        Tag tag1 = new Tag("Kokk");
        Tag tag2 = new Tag("Kaptein");
        Tag tag3 = new Tag("Kelner");

        context.Tags.Add(tag1);
        context.Tags.Add(tag2);
        context.Tags.Add(tag3);

        User user1 = new User("Kennedy", "kennedy@itu.dk");
        User user2 = new User("AppleBottom", "applebottom@itu.dk");

        context.Users.Add(user1);
        context.Users.Add(user2);


        context.SaveChanges();

        _context = context;
        _repo = new TaskRepository(_context);

    }

    [Fact]
    public void create_task_given_task_returns_correct_response_and_id()
    {
        //Arrange
        var task = new TaskCreateDTO("Lave aftensmad", 3, "Det er hyggelig", new[] { (new Tag("Mad")).Name });

        //Act
        var actual = _repo.Create(task);
        var expected = (Response.Created, 3);
        //Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void test_read_given_ID_out_of_bounds_returns_null()
    {
        var input = 5;
        var result = _repo.Read(input);

        Assert.Null(result);
    }


    [Fact]
    public void read_by_state_returns_tasks_with_state_new()
    {
        // Arrange & Act
        var result = _repo.ReadAllByState(State.New).Select(x => x.Title);
        var expected = new string[] { "Lave aftensmad" };

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
    [Fact]
    public void delete_given_ID_out_of_bounds_return_not_found()
    {
        var result = _repo.Delete(55);
        Assert.Equal(Response.NotFound, result);
    }

    [Fact]
    public void deleteTask_given_taskId_returns_Deleted()
    {
        var deleted = _repo.Delete(2);
        Assert.Equal(Response.Deleted, deleted);

    }

    /* 
    Tags which are assigned to a task may only be deleted using the force.
    Trying to delete a tag in use without the force should return Conflict.
    Trying to create a tag which exists already should return Conflict. */

    [Fact]
    public void Dispose()
    {
        _context.Dispose();
    }
}
