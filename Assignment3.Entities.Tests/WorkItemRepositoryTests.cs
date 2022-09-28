using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Assignment3.Core;
namespace Assignment3.Entities.Tests;

public sealed class WorkItemRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly WorkItemRepository _repository;
    public WorkItemRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        var User1 = new User { Id = 1, Name = "Superman", Email = "superman123@gmail.com" };
        var tag1 = new Tag {id = 1, Name = "tag number 1"};
        var WorkItem1 = new WorkItem("Title work Item 1", User1, "Detailed description!",  new HashSet<Tag>{tag1}) {Id = 1};
        context.WorkItems.AddRange(WorkItem1);
        context.Users.Add(User1);
        context.Tags.Add(tag1);
        context.SaveChanges();

        _context = context;
        _repository = new WorkItemRepository(_context);
    }

    [Fact]
    public void Test_create_workitem()
    {
        //Arrange
        
        //Act
        var (response, id) = _repository.Create(new WorkItemCreateDTO("Work Item 2", 1, "Very detailed description", new string[]{"tag number 1"}));

        //Assert
        response.Should().Be(Response.Created);
        id.Should().Be(2);
        
    }

    [Fact]
    public void Test_create_workitem_returns_NotFound()
    {
        //Arrange
        
        //Act
        var (response, id) = _repository.Create(new WorkItemCreateDTO("Work Item 2", 4, "Very detailed description", new string[]{"tag number 1"}));

        //Assert
        response.Should().Be(Response.NotFound);
        id.Should().Be(0);
        
    }

    [Fact]
    public void Test_read_all_workitems()
    {
        //Arrange
        
        //Act
     

        //Assert
     
        
    }

        [Fact]
    public void Test_read_all_removed()
    {
        //Arrange
        
        //Act
     

        //Assert
     
        
    }

        [Fact]
    public void Test_read_all_by_tag()
    {
        //Arrange
        
        //Act
     

        //Assert
     
        
    }
        [Fact]
    public void Test_read_all_by_user()
    {
        //Arrange
        
        //Act
     

        //Assert
     
        
    }

        [Fact]
    public void Test_read_all_by_state()
    {
        //Arrange
        
        //Act
     

        //Assert
     
        
    }

        [Fact]
    public void Test_read_one_workitem()
    {
        //Arrange
        
        //Act
     

        //Assert
     
        
    }

        [Fact]
    public void Test_update_workitem()
    {
        //Arrange
        
        //Act
     

        //Assert
     
        
    }

        [Fact]
    public void Test_delete_workitem()
    {
        //Arrange
        
        //Act
     

        //Assert
     
        
    }
    public void Dispose()
    {
        _context.Dispose();
    }

}
