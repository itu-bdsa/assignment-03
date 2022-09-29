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

    /*public (Response Response, int TaskId) Create2(TaskCreateDTOEasy task)
    {
        var entry = new Task(task.Title, State.New) { Id = context.Tasks.Count() + 1, State = State.New, Created = DateTime.UtcNow, StateUpdated = DateTime.UtcNow, Tags = context.Tags.Where(x => task.Tags.Contains(x.Name)).ToList() };
        context.Tasks.Add(entry);
        context.SaveChanges();


        return (Response.Created, entry.Id);
    }*/

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        return context.Tasks.Select(t => new TaskDTO(t.Id, t.Title, t.AssignedTo.name, t.Tags.Select(x => x.Name).ToArray(), t.State)).ToList().AsReadOnly();
    }
    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        return context.Tasks.Select(t => new TaskDTO(t.Id, t.Title, t.AssignedTo.name, t.Tags.Select(x => x.Name).ToArray(), State.Removed)).ToList().AsReadOnly();
    }
    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        var tasks = from t in context.Tasks
                    where t.Tags.Select(x => x.Name).Contains(tag)
                    select new TaskDTO(t.Id, t.Title, t.AssignedTo.name, t.Tags.Select(x => x.Name).ToArray(), t.State);
        return tasks.ToArray();
    }
    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        var tasks = from t in context.Tasks
                    where t.AssignedTo.id == userId
                    select new TaskDTO(t.Id, t.Title, t.AssignedTo.name, t.Tags.Select(x => x.Name).ToArray(), t.State);
        return tasks.ToArray();
    }
    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        var tasks = from t in context.Tasks
                    where t.State == state
                    select new TaskDTO(t.Id, t.Title, t.AssignedTo.name, t.Tags.Select(x => x.Name).ToArray(), t.State);
        return tasks.ToArray();
    }
    public TaskDetailsDTO Read(int taskId)
    {
        var task = context.Tasks.Find(taskId);
        if (task != null)
        {
            return new TaskDetailsDTO(task.Id, task.Title, task.Description, task.Created, task.AssignedTo.name, task.Tags.Select(x => x.Name).ToArray(), task.State, task.StateUpdated);
        }
        else
        {
            return null;
        }
    }
    public Response Update(TaskUpdateDTO task)
    {
        var t = context.Tasks.Find(task.Id);
        if (t != null)
        {
            t.Title = task.Title;
            t.Tags = context.Tags.Where(x => task.Tags.Contains(x.Name)).ToList();
            t.StateUpdated = DateTime.UtcNow;
            context.SaveChanges();
            return Response.Updated;
        }
        else
        {
            return Response.NotFound;
        }
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
