namespace Assignment3.Entities;
using Assignment3.Core;

public class TagRepository : ITagRepository
{

    private readonly KanbanContext _context;

    public TagRepository(KanbanContext context)
    {
        _context = context;
    }
//Tags which are assigned to a workItem may only be deleted using the force.
//Trying to delete a tag in use without the force should return Conflict.
//Trying to create a tag which exists already should return Conflict.
    public (Response Response, int TagId) Create(TagCreateDTO tag){
        var entity = _context.Tags.FirstOrDefault(c => c.Name == tag.Name);
        Response response;
        return (Response.BadRequest, 0);
    }

    public IReadOnlyCollection<TagDTO> ReadAll() {
        return null;
    }

    public TagDTO Read(int tagID) {
        return null;
    }

    public Response Update(TagUpdateDTO tag) {
        return Response.BadRequest;
    }

    public Response Delete(int tagID, bool force = false) {
        return Response.BadRequest;
    }
}

