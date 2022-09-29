namespace Assignment3.Entities;
using Assignment3.Core;

public class WorkItemRepository : IWorkItemRepository
{

    private readonly KanbanContext _context;

    public WorkItemRepository(KanbanContext context)
    {
        _context = context;
    }

    public (Response Response, int WorkItemId) Create(WorkItemCreateDTO workItem) 
    {
        var entity = _context.WorkItems.FirstOrDefault(c => c.Title == workItem.Title);
        Response response;

        if (entity is null)
        {
            var assignedToTemp = _context.Users.FirstOrDefault(c => c.Id == workItem.AssignedToId.Value);
            
            if (assignedToTemp is null && workItem.AssignedToId.Value is not 0){
                return (Response.NotFound, 0);
            } 
            
            else{
                ICollection<Tag> tagsTemp = new List<Tag>();
                if (workItem.Tags is not null && _context.Tags is not null){
                    tagsTemp = _context.Tags.Where(t => workItem.Tags.Any(x => x == t.Name)).ToArray();
                }
                entity = new WorkItem{Title = workItem.Title, AssignedTo = assignedToTemp, Description = workItem.Description, Tags = tagsTemp};

                _context.WorkItems.Add(entity);
                _context.SaveChanges();

                response = Response.Created;
            }
        }
        else
        {
            response = Response.Conflict;
        }
        return (response, entity.Id);
    }
    public IReadOnlyCollection<WorkItemDTO> ReadAll()
    {
        var workItems = from c in _context.WorkItems
        orderby c.Title
        select new WorkItemDTO(c.Id, c.Title, c.AssignedTo.Name, c.Tags.Select(c => c.Name).ToArray(), c.state);

        return workItems.ToArray();
    }
    public IReadOnlyCollection<WorkItemDTO> ReadAllRemoved() 
    {
        var removedWorkItems = from c in _context.WorkItems
        where c.state == State.Removed
        orderby c.Title
        select new WorkItemDTO(c.Id, c.Title, c.AssignedTo.Name, c.Tags.Select(c => c.Name).ToArray(), c.state);

        return removedWorkItems.ToArray();
    }
    public IReadOnlyCollection<WorkItemDTO> ReadAllByTag(string tag) 
    {
        var tagWorkItems = from c in _context.WorkItems
        where c.Tags.Any(x => x.Name == tag)
        orderby c.Title
        select new WorkItemDTO(c.Id, c.Title, c.AssignedTo.Name, c.Tags.Select(c => c.Name).ToArray(), c.state);

        return tagWorkItems.ToArray();
    }
    public IReadOnlyCollection<WorkItemDTO> ReadAllByUser(int userId)
    {
        var userWorkItems = from c in _context.WorkItems
        where c.AssignedTo.Id == userId
        orderby c.Title
        select new WorkItemDTO(c.Id, c.Title, c.AssignedTo.Name, c.Tags.Select(c => c.Name).ToArray(), c.state);

        return userWorkItems.ToArray();
    }
    public IReadOnlyCollection<WorkItemDTO> ReadAllByState(State state)
    {
        var tagWorkItems = from c in _context.WorkItems
        where c.state == state
        orderby c.Title
        select new WorkItemDTO(c.Id, c.Title, c.AssignedTo.Name, c.Tags.Select(c => c.Name).ToArray(), c.state);

        return tagWorkItems.ToArray();
    }
    public WorkItemDetailsDTO Read(int workItemId)
    {
        var workItemDetails = from c in _context.WorkItems
        where c.Id == workItemId 
        select new WorkItemDetailsDTO(c.Id, c.Title, c.Description, c.Created, c.AssignedTo.Name, c.Tags.Select(c => c.Name).ToArray(), c.state, c.StateUpdated);
        return workItemDetails.FirstOrDefault();
    }
    public Response Update(WorkItemUpdateDTO WorkItem) 
    {
       var entity = _context.WorkItems.Find(WorkItem.Id);
       Response response;
        if (entity is null)
        {
            response = Response.NotFound;
        }
         else if (_context.WorkItems.FirstOrDefault(w => w.Id != WorkItem.Id && w.Title == WorkItem.Title) != null)
        {
            response = Response.Conflict;
        }
        else
        {
            entity.Title = WorkItem.Title;
            if (WorkItem.AssignedToId is not null)
            {
                var temp = _context.Users.Where(c => c.Id == WorkItem.AssignedToId.Value).FirstOrDefault();
                if (temp is null) {response = Response.BadRequest;}
                else {entity.AssignedTo = temp;}
            }
            
            if (WorkItem.Description != null) {entity.Description = WorkItem.Description;}

            entity.Tags = _context.Tags.Where(t => WorkItem.Tags.Any(x => x == t.Name)).ToArray();

            if (entity.state != WorkItem.State){
                entity.state = WorkItem.State;
                entity.StateUpdated = DateTime.UtcNow;
            }
                
            _context.SaveChanges();
            response = Response.Updated;
        }
        return response;
    }

    public Response Delete(int workItemId)
    {
        var entity = _context.WorkItems.Find(workItemId);
       Response response;
        if (entity is null)
        {
            response = Response.NotFound;
        }
        else{
            switch (entity.state)
            {
            case State.Removed: case State.Closed: case State.Resolved:
                response = Response.Conflict;
            break;

            case State.Active:
                entity.state = State.Removed;
                entity.StateUpdated = DateTime.UtcNow;
                _context.SaveChanges();
                response = Response.Updated;
            break;

            case State.New:
                _context.WorkItems.Remove(entity);
                _context.SaveChanges();
                response = Response.Deleted;
            break;

            default:
                response = Response.BadRequest;
                break;
            }
        }
         
        return response;
    }
    

}
