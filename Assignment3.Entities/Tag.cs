namespace Assignment3.Entities;

public class Tag
{   
    public int id {get; set;}
    public string Name {get; set;}
    public ICollection<WorkItem> WorkItems {get; set;} = new List<WorkItem>();

    // public Tag(string name)
    // {
    //     WorkItems = new List<WorkItem>();
    //     Name = name;
    // }
}

