namespace Assignment3.Entities;

public class Tag
{
    public Tag(string name)
    {
        this.Name = name;
    }
    
    public int Id {get; set;}

    [StringLength(50)]
    public string Name {get; set;}

    public ICollection<Task> Tasks {get; set;} = null!;

}
