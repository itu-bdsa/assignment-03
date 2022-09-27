using Assignment3.Core;

namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    public readonly KanbanContext context;

    public TaskRepository(KanbanContext _context)
    {
        context = _context;
    }

    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        var entity = new Task { title = task.Title };

        context.Tasks.Add(entity);
        context.SaveChanges();

        return (Response.Created, entity.id);

    }

    public Response Delete(int taskId)
    {
        var taskToDelete = context.Tasks.FirstOrDefault(t => t.id == taskId);

        if (taskToDelete == null)
        {
            return Response.NotFound;
        }
        else
        {
            context.Tasks.Remove(taskToDelete);
            context.SaveChanges();

            return Response.Deleted;
        }


    }

    public TaskDetailsDTO Read(int taskId)
    {
        var taskToRead = from c in context.Tasks
                         where c.id == taskId
                         select new TaskDetailsDTO(c.id, c.title, c.description, new DateTime().AddYears(-5), c.assignedTo.name, (IReadOnlyCollection<string>)c.tags, c.state, new DateTime().AddDays(2));


        return taskToRead.FirstOrDefault();
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        var tasks = from c in context.Tasks 
                    orderby c.title 
                    select new TaskDTO(c.id, c.title, c.assignedTo.name, (IReadOnlyCollection<string>)c.tags, c.state);
        
        return tasks.ToArray();

    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        var tasks = from c in context.Tasks
                    where c.state == state
                    orderby c.title
                    select new TaskDTO(c.id, c.title, c.assignedTo.name, (IReadOnlyCollection<string>)c.tags, c.state);
        
        return tasks.ToArray();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        var tasks = from c in context.Tasks
                    where c.tags.Any(t => t.name == tag)
                    orderby c.title
                    select new TaskDTO(c.id, c.title, c.assignedTo.name, (IReadOnlyCollection<string>)c.tags, c.state);
        
        return tasks.ToArray();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        var tasks = from c in context.Tasks
                    where c.assignedTo.id == userId
                    orderby c.title
                    select new TaskDTO(c.id, c.title, c.assignedTo.name, (IReadOnlyCollection<string>)c.tags, c.state);
        
        return tasks.ToArray();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        var tasks = from c in context.Tasks
                    where c.state == State.Removed
                    orderby c.title
                    select new TaskDTO(c.id, c.title, c.assignedTo.name, (IReadOnlyCollection<string>)c.tags, c.state);
        
        return tasks.ToArray();
    }

    public Response Update(TaskUpdateDTO task)
    {
        var taskToUpdate = context.Tasks.FirstOrDefault(t => t.id == task.Id);

        if (taskToUpdate == null)
        {
            return Response.NotFound;
        }
        else
        {
            taskToUpdate.title = task.Title;
            taskToUpdate.description = task.Description;
            taskToUpdate.state = task.State;
            

            context.SaveChanges();

            return Response.Updated;
        }
    }
}
