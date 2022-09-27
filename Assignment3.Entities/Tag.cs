namespace Assignment3.Entities;

public class Tag
{   
    public int id {get; set;}
    public string Name {get; set;}
    public ICollection<WorkItem> WorkItems {get; set;}

}

