namespace Assignment3.Entities;

public class User
{
    public User(string name, string email)
    {
        this.name = name;
        this.email = email;
    }

    public int id { get; set; }

    [StringLength(100)]
    public string name { get; set; }

    [StringLength(100)]
    [EmailAddress]
    public string email { get; init; }

    public ICollection<Task> tasks { get; set; } = null!;

}
