namespace Assignment3.Entities.Tests;

public class UserRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly UserRepository _repo;
    public UserRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();

        var _taskList = new List<Task> {new Task("repetitive coding", State.Active)};
        var _user = new User("Sir Phil", "em@i.l");
        _user.Tasks = _taskList;
        context.Users.Add(_user);

        context.SaveChanges();

        _context = context;
        _repo = new UserRepository(_context);

        

    }

    [Fact]
    public void delete_assigned_user_no_force_returns_conflict()
    {
        var result = _repo.Delete(1);
        Assert.Equal(Response.Conflict,result);
    }

    [Fact]
    public void delete_assigned_user_with_force_returns_deleted()
    {
        var result = _repo.Delete(1, true);
        Assert.Equal(Response.Deleted,result);
    }

    //Trying to create a user which exists already (same email) should return Conflict

    [Fact]
    public void create_snd_user_with_same_mail_return_conflict()
    {
        var user = new UserCreateDTO("Jeanne", "em@i.l");
        var result = _repo.Create(user);

        Assert.Equal((Response.Conflict,1),result);
    }
}
