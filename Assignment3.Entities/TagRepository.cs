namespace Assignment3.Entities;
using Assignment3.Core;

public class TagRepository : ITagRepository
{

    private readonly KanbanContext _context;

    public TagRepository(KanbanContext context)
    {
        _context = context;
    }



    public (Response Response, int TagId) Create(TagCreateDTO tag){
        var entity = (from c in _context.Tags
                        where c.Name == tag.Name
                        select c).FirstOrDefault();

        if (entity is null)
        {
            entity = new Tag{Name = tag.Name};

            _context.Tags.Add(entity);
            _context.SaveChanges();

            return (Response.Created, entity.id);
        }
        else
        {
            return (Response.Conflict, -1);
        }
    }

    public IReadOnlyCollection<TagDTO> ReadAll() {
        var tags = from c in _context.Tags
                 select new TagDTO(c.id, c.Name);
        return tags.ToArray();
    }

    public TagDTO Read(int tagID) {
        var tag = (from c in _context.Tags
                 where c.id == tagID
                 select c).FirstOrDefault();
        if (tag is null)
        {
            return null;
        }
        else{
            return new TagDTO(tag.id, tag.Name);
        }
    }

    public Response Update(TagUpdateDTO tag) {
        var entity = _context.Tags.Find(tag.Id);

        if (entity is null)
        {
            return Response.NotFound;
        }
        else
        {
            entity.Name = tag.Name;
            _context.SaveChanges();
            return Response.Updated;
        }
    }

    public Response Delete(int tagID, bool force = false) {
        var tag = (from c in _context.Tags
                 where c.id == tagID
                 select c).FirstOrDefault();
        if (tag is null)
        {
            return Response.NotFound;
        }
        else if (tag.WorkItems.Any())
        {
            if (force){
                _context.Tags.Remove(tag);
                _context.SaveChanges();
                return Response.Deleted;
            }
            return Response.Conflict;
        }
        _context.Tags.Remove(tag);
        _context.SaveChanges();
        return Response.Deleted;
    }
}

