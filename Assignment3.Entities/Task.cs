namespace Assignment3.Entities;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public User AssignedTo { get; set; } = null!;
    public string Description { get; set; } = null!;
    public StateEnum State { get; set; }
    public ICollection<Tag> Tags { get; set; }
}

public enum StateEnum
{
    New,
    Active,
    Resolved,
    Closed,
    Removed,
}
