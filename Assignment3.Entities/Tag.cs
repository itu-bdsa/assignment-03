namespace Assignment3.Entities;

public class Tag
{
    public Tag(string name)
    {
        Name = name;
    }

    public int Id { get; set; }
    [Required, StringLength(50)]
    public string Name { get; set; }
    public List<Task>? Tasks { get; set; }
}
