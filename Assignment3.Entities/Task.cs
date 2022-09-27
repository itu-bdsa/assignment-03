namespace Assignment3.Entities;

public class Task
{
    public int id { get; init; }
    public string title { get; set; }
    public User assignedTo { get; set; }
    public string? description { get; set; }
    public State state { get; set; }
    public List<Tag> tags { get; set; }

}
