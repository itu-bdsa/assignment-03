using Assignment3.Core;
namespace Assignment3.Entities;

public class TagRepository : ITagRepository
{
    private readonly KanbanContext context;

    public TagRepository(KanbanContext _context)
    {
        context = _context;
    }

    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {
        var entity = context.Tags.FirstOrDefault(c => c.name == tag.Name);
        Response response;

        if (entity is null)
        {
            entity = new Tag { name = tag.Name};

            context.Tags.Add(entity);
            context.SaveChanges();

            response = Response.Created;
        }
        else
        {
            response = Response.Conflict;
        }

        return (response, entity.id);
    }

    public Response Delete(int tagId, bool force = false)
    {
        var entity = context.Tags.Find(tagId);

        if (entity == null)
        {
            return Response.NotFound;
        }

        if (entity.tasks.Count > 0 && !force)
        {
            return Response.Conflict;
        }

        context.Tags.Remove(entity);
        context.SaveChanges();

        return Response.Deleted;
    }

    public TagDTO Read(int tagId)
    {
        var entity = context.Tags.Find(tagId);

        if (entity == null)
        {
            return null;
        }

        return new TagDTO(entity.id, entity.name);
    }

    public IReadOnlyCollection<TagDTO> ReadAll()
    {
        var tags = from c in context.Tags
                   orderby c.name
                   select new TagDTO(c.id, c.name);

        return tags.ToArray();
    }

    public Response Update(TagUpdateDTO tag)
    {
        var entity = context.Tags.Find(tag.Id);

        if (entity == null)
        {
            return Response.NotFound;
        }

        entity.name = tag.Name;

        context.SaveChanges();

        return Response.Updated;
    }
}
