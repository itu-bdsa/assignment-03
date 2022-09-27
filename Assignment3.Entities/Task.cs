namespace Assignment3.Entities;

public class Task
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? Title { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public string? Description { get; set; }
    [Required]
    public State State { get; set; }
    public ICollection<Tag>? Tags { get; set; }
}
