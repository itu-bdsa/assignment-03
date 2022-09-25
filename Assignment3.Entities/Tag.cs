using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using Assignment3.Core;

namespace Assignment3.Entities;

public class Tag : ITagRepository
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Task> Tasks { get; set; }

    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {
        if (this.Name.Equals(tag.Name))
            return (Response.Conflict, this.Id);
        return (Response.Created, this.Id);                         // Should maybe be switched to another Id
    }

    public IReadOnlyCollection<TagDTO> ReadAll()
    {
        var temp = new Collection<TagDTO>();
        
        foreach (var task in Tasks)
        {
            foreach (var tag in task.Tags)
            {
                temp.Add(new TagDTO(tag.Id, tag.Name));
            }
        }

        return new ReadOnlyCollection<TagDTO>(temp);
    }

    public TagDTO Read(int tagId)
    {
        foreach (var tag in ReadAll())
        {
            if (tag.Id == tagId)
            {
                return tag;
            }
        }

        return new TagDTO(-1, "");
    }

    public Response Update(TagUpdateDTO tag)
    {
        if (this.Name.Equals(tag.Name) && this.Id == tag.Id)
            return Response.BadRequest;
        this.Id = tag.Id;
        this.Name = tag.Name;
        return Response.Updated;
    }

    public Response Delete(int tagId, bool force = false)
    {
        throw new NotImplementedException();
    }
}

