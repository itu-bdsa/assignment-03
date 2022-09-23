namespace Assignment3.Entities;

public partial class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public User AssignedTo {get; set;} = null!;
    public string Description { get; set; } = null!;
    public State state { get; set; }
    public List<Tag> Tags {get; set;}
    
}
public enum State{
    New,
    Active,
    Resolved,
    Closed,
    Removed
}
