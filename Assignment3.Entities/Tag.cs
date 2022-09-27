namespace Assignment3.Entities;

public class Tag
{
    public int id { get; init; }
    public string name { get; set; }
    public List<Task> tasks { get; set; }
}
