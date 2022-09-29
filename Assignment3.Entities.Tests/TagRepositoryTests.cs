namespace Assignment3.Entities.Tests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Assignment3.Entities;
using Assignment3.Core;

public sealed class TagRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly ITagRepository _repository;

    public TagRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        var item3 = new WorkItem{Title = "item1", state = State.New};
        var item4 = new WorkItem{Title = "item2", state = State.New};
        var tag1 = new Tag{Name = "tagName1"};
        var tag2 = new Tag{Name = "tagName2"};
        var tag3 = new Tag{Name = "tagName3", WorkItems = new List<WorkItem>{item3}};
        var tag4 = new Tag{Name = "tagName4", WorkItems = new List<WorkItem>{item4}};

        context.Tags.AddRange(tag1, tag2, tag3, tag4);
        context.SaveChanges();
        _context = context;

        _repository = new TagRepository(_context);
    }

    [Fact]
    public void update_non_existing_tag_should_return_NotFound()=>_repository.Update(new TagUpdateDTO(5, "tagName5")).Should().Be(Response.NotFound);

    [Fact]
    public void update_existing_tag_should_return_Updated()=>_repository.Update(new TagUpdateDTO(1, "tagName1")).Should().Be(Response.Updated);

    [Fact]
    public void delete_non_existing_tag_should_return_NotFound()=>_repository.Delete(5).Should().Be(Response.NotFound);

    [Fact]
    public void delete_existing_tag_without_any_workItems_should_return_deleted() => _repository.Delete(1).Should().Be(Response.Deleted);

    [Fact]
    public void delete_existing_tag_with_workItems_and_force_return_deleted() =>_repository.Delete(4, true).Should().Be(Response.Deleted);
    

    [Fact]
    public void delete_existing_tag_with_workItems_and_no_force_return_Conflict()=>_repository.Delete(3).Should().Be(Response.Conflict);
    

    [Fact]
    public void create_new_tag_should_return_Created_and_tag_id() => _repository.Create(new TagCreateDTO("tagName5")).Should().Be((Response.Created, 5));

    [Fact]
    public void create_already_existing_tag_should_return_Conflict_and_minus_one() => _repository.Create(new TagCreateDTO("tagName2")).Should().Be((Response.Conflict, -1));
    
    [Fact]
    public void read_id_1_should_return_tag_dto_with_id_1_and_name_tagName1() => _repository.Read(1).Should().Be(new TagDTO(1,"tagName1"));

    [Fact]
    public void read_id_5_should_return_null() => _repository.Read(5).Should().Be(null);

    [Fact]
    public void readAll_should_return_four_tags() => _repository.ReadAll().Should().Equal(new TagDTO[]{ new TagDTO(1,"tagName1"),
                                                                                                     new TagDTO(2, "tagName2"),
                                                                                                     new TagDTO(3, "tagName3"),
                                                                                                     new TagDTO(4, "tagName4")});

    public void Dispose()
    {
        _context.Dispose();
    }
}
