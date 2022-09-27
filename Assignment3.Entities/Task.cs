namespace Assignment3.Entities;
using Assignment3.Core;

public class Task
{
    public Task(string title, State state)
    {
        this.Title = title;
        this.State = state;
    }
    public int Id {get ; set;}

    [Required]
    [StringLength(100)]
    public string Title {get; set;}

    public User? AssignedTo {get; set;}

    public string? Description {get; set;}

    [Required]
    public State State {get; set;}
    
    public ICollection<Tag> Tags {get; set;} = null!;
}
