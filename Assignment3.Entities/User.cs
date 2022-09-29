namespace Assignment3.Entities;

public class User
{
    public User(string name, string email)
    {
        this.Name = name;
        this.Email = email;
    }

    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(100)]
    [EmailAddress]
    public string Email { get; init; }

    public ICollection<Task> Tasks { get; set; } = null!;

}
