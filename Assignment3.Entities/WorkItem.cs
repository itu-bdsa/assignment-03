namespace Assignment3.Entities;
using Assignment3.Core;

public partial class WorkItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public User? AssignedTo {get; set;}
    public string? Description { get; set; }
    public State state { get; set; }
    public ICollection<Tag> Tags {get; set;} = new List<Tag>();
}
