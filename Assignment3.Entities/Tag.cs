using System.Diagnostics.Tracing;
using Assignment3.Core;

namespace Assignment3.Entities;

public class Tag : ITagRepository
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Task> Tasks { get; set; }

    (Response Response, int TagId) Create(TagCreateDTO tag)
    {
        
    }
    IReadOnlyCollection<TagDTO> ReadAll();
    TagDTO Read(int tagId);
    Response Update(TagUpdateDTO tag);
    Response Delete(int tagId, bool force = false);
}

