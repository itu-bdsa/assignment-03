namespace Assignment3.Entities;
using Assignment3.Core;

public partial class WorkItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public User AssignedTo {get; set;} = null!;
    public string Description { get; set; } = null!;
    public State state { get; set; }
    public ICollection<Tag> Tags {get; set;}
    public DateTime Created {get; set;}
    public DateTime StateUpdated {get; set;}
    
    public WorkItem(){
        
    }
    public WorkItem(string title, User assignedTo, string description, ICollection<Tag> tags)
    {
        Title = title;
        AssignedTo = assignedTo;
        Description = description;
        Tags = tags;
        state = State.New;
        Created = DateTime.UtcNow;
        StateUpdated = DateTime.UtcNow;
    }
}
