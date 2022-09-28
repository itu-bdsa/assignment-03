namespace Assignment3.Entities;

public class TaskRepository
{
    private readonly IKanbanContext context;

    public TaskRepository(IKanbanContext context)
    {
        this.context = context;
    }

    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        var entry = new Task(task.Title, State.New) { Id = context.Tasks.Count() + 1, State = State.New, Created = DateTime.UtcNow, StateUpdated = DateTime.UtcNow, Tags = context.Tags.Where(x => task.Tags.Contains(x.Name)).ToList() };
        context.Tasks.Add(entry);
        context.SaveChanges();


        return (Response.Created, entry.Id);
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        throw new NotImplementedException();
    }
    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        throw new NotImplementedException();
    }
    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        throw new NotImplementedException();
    }
    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        throw new NotImplementedException();
    }
    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        throw new NotImplementedException();
    }
    public TaskDetailsDTO Read(int taskId)
    {
        throw new NotImplementedException();
    }
    public Response Update(TaskUpdateDTO task)
    {
        throw new NotImplementedException();
    }
    public Response Delete(int taskId)
    {
        var task = context.Tasks
            .Where(t => t.Id == taskId)
            .Include(t => t.Tags)
            .FirstOrDefault(t => t.Id == taskId);

        if (task != null)
        {
            if (task.State == State.Active) //state not new
            {
                task.State = State.Removed;
                return Response.Deleted;
            }
            else if (task.State != State.New)
            {
                return Response.Conflict;
            }
            else
            {
                context.Tasks.Remove(task);
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
