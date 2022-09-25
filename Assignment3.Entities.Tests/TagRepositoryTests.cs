using System.Collections.ObjectModel;
using System.Security.Cryptography;
using Assignment3.Core;

namespace Assignment3.Entities.Tests;

public class TagRepositoryTests
{
    [Fact]
    public void CreateAlreadyExistingTagShouldReturnConflictAndId()
    {
        // Arrange
        Tag tag = new Tag();
        tag.Name = "MyName";
        
        TagCreateDTO record = new TagCreateDTO("MyName");
        
        // Act
        var output = tag.Create(record);
        
        // Assert
        Assert.Equal((Response.Conflict, tag.Id), output);
    }

    [Fact]
    public void CreateDoesNotExistTagShouldReturnCreatedAndId()
    {
        // Arrange
        Tag tag = new Tag();
        tag.Name = "Bo";
        TagCreateDTO record = new TagCreateDTO("MyName");
        
        // Act
        var output = tag.Create(record);
        
        // Assert
        Assert.Equal((Response.Created, tag.Id), output);
    }

    [Fact]
    public void ReadAllShouldReturnReadCollectionOfAllTasksTags()
    {
        // Arrange
        Tag dad = new Tag();
        Tag son1 = new Tag();
        Tag son2 = new Tag();
        Tag son3 = new Tag();
        Tag son4 = new Tag();
        son1.Name = "Son";
        son1.Id = 1;
        son2.Name = "Sonn";
        son2.Id = 2;
        son3.Name = "Sonny";
        son3.Id = 3;
        son4.Name = "SonnyB";
        son4.Id = 4;

        Task task1 = new Task();
        Task task2 = new Task();

        var list1 = new Collection<Tag>();
        var list2 = new Collection<Tag>();
        
        list1.Add(son1);
        list2.Add(son2);
        list2.Add(son3);
        list2.Add(son4);

        task1.Tags = list1;
        task2.Tags = list2;

        var tasks = new Collection<Task>();
        tasks.Add(task1);
        tasks.Add(task2);

        dad.Tasks = tasks;

        var list = new Collection<TagDTO>();
        list.Add(new TagDTO(son1.Id, son1.Name));
        list.Add(new TagDTO(son2.Id, son2.Name));
        list.Add(new TagDTO(son3.Id, son3.Name));
        list.Add(new TagDTO(son4.Id, son4.Name));
        var expected = new ReadOnlyCollection<TagDTO>(list);
        
        // Act
        var output = dad.ReadAll();
        
        // Assert
        Assert.Equal(expected, output);
    }

    [Fact]
    public void ReadShouldOnlyReturnSon1Given1()
    {
        // Arrange
        Tag dad = new Tag();
        Tag son1 = new Tag();
        Tag son2 = new Tag();
        Tag son3 = new Tag();
        Tag son4 = new Tag();
        son1.Name = "Son";
        son1.Id = 1;
        son2.Name = "Sonn";
        son2.Id = 2;
        son3.Name = "Sonny";
        son3.Id = 3;
        son4.Name = "SonnyB";
        son4.Id = 4;

        Task task1 = new Task();
        Task task2 = new Task();

        var list1 = new Collection<Tag>();
        var list2 = new Collection<Tag>();
        
        list1.Add(son1);
        list2.Add(son2);
        list2.Add(son3);
        list2.Add(son4);

        task1.Tags = list1;
        task2.Tags = list2;

        var tasks = new Collection<Task>();
        tasks.Add(task1);
        tasks.Add(task2);

        dad.Tasks = tasks;
        
        // Act
        var actual = dad.Read(1);
        
        // Assert
        Assert.Equal(new TagDTO(1, "Son"), actual);
    }
    
    [Fact]
    public void ReadShouldReturnNegativeOneGiven5()
    {
        // Arrange
        Tag dad = new Tag();
        Tag son1 = new Tag();
        Tag son2 = new Tag();
        Tag son3 = new Tag();
        Tag son4 = new Tag();
        son1.Name = "Son";
        son1.Id = 1;
        son2.Name = "Sonn";
        son2.Id = 2;
        son3.Name = "Sonny";
        son3.Id = 3;
        son4.Name = "SonnyB";
        son4.Id = 4;

        Task task1 = new Task();
        Task task2 = new Task();

        var list1 = new Collection<Tag>();
        var list2 = new Collection<Tag>();
        
        list1.Add(son1);
        list2.Add(son2);
        list2.Add(son3);
        list2.Add(son4);

        task1.Tags = list1;
        task2.Tags = list2;

        var tasks = new Collection<Task>();
        tasks.Add(task1);
        tasks.Add(task2);

        dad.Tasks = tasks;
        
        // Act
        var actual = dad.Read(5);
        
        // Assert
        Assert.Equal(new TagDTO(-1, ""), actual);
    }
    
    
}
