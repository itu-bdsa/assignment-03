namespace Assignment3.Entities;

public class WorkItemRepository
{

    private readonly KanbanContext _context;

    public WorkItemRepository(KanbanContext context)
    {
        _context = context;
    }
}
