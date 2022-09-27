namespace Assignment3.Entities;
using Assignment3.Core;
using Assignment3;

public class TagRepository : ITagRepository
{

    private readonly IKanbanContext context;

    public TagRepository(IKanbanContext context)
    {
        this.context = context;
    }
    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {
        var entry = new Tag(tag.Name);
        context.Tags.Add(entry);
        context.SaveChanges();

        return (Response.Created, entry.Id);
    }
    public IReadOnlyCollection<TagDTO> ReadAll()
    {
        return context.Tags.Select(e => new TagDTO(e.Id, e.Name)).ToList().AsReadOnly();
    }
    public TagDTO Read(int tagId)
    {
        var e = context.Tags.Find(tagId);

        if (e == null)
        {
            return null!;
        }
        else
        {
            return new TagDTO(e!.Id, e.Name);
        }

    }
    public Response Update(TagUpdateDTO tag)
    {
        var e = context.Tags.Find(tag.Id);
        if (e != null)
        {
            e.Name = tag.Name;
            context.SaveChanges();
            return Response.Updated;
        }
        else
        {
            return Response.NotFound;
        }

    }
    public Response Delete(int tagId, bool force = false)
    {
        var e = context.Tags
            .Where(tg => tg.Id == tagId)
            .Include(tg => tg.Tasks)
            .FirstOrDefault();

        if (e != null)
        {

            if ((e.Tasks.Count) !=0 )
            {
                if (force) //assigned and forced
                {
                    context.Tags.Remove(e);
                    context.SaveChanges();
                    return Response.Deleted;
                }
                else //assigned no force
                {
                    return Response.Conflict;
                }
            }
            else //not assigned
            {
                context.Tags.Remove(e);
                context.SaveChanges();
                return Response.Deleted;
            }
        }
        else
        {
            return Response.NotFound;
        }
    }
}
