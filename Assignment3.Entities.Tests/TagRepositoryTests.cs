namespace Assignment3.Entities.Tests;
using Assignment3.Core;
using Assignment3;
public class TagRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly TagRepository _repo;

    public TagRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        var _task = new Task("repetitive coding", State.Active);
        var _tag = new Tag("Sir Phil");
        _tag.Tasks = new List<Task>{_task};
        context.Tags.Add(new Tag("Smadre manden"));
        context.Tags.Add(_tag);

        context.SaveChanges();

        _context = context;
        _repo = new TagRepository(_context);

    }

    [Fact]
    public void test_create()
    {
        //Arrange
        var input = new TagCreateDTO("Er det Hanne?");

        /*//DOCKER
        var factory = new KanbanContextFactory();
        var context = factory.CreateDbContext(new string[0]); */
        //PASSED 

        //Act
        var result = _repo.Create(input);
        //Assert

        var expect = (Response.Created, 3);

        Assert.Equal(expect, result);
    }

    [Fact]
    public void test_read_given_ID_out_of_bounds_returns_null()
    {
        var input = 5;
        var result = _repo.Read(input);

        Assert.Null(result);
    }

    [Fact]
    public void delete_given_ID_out_of_bounds_return_not_found()
    {
        var result = _repo.Delete(55);
        Assert.Equal(Response.NotFound, result);
    }

    [Fact]
    public void delete_assigned_tag_no_force_returns_conflict()
    {
        var result = _repo.Delete(2);
        Assert.Equal(Response.Conflict,result);
    }

        [Fact]
    public void delete_assigned_tag_with_force_returns_deleted(){
        var result = _repo.Delete(2, true);
        Assert.Equal(Response.Deleted,result);
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
