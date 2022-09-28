namespace Assignment.Infrastructure;

public class Tag
{
  public int Id { get; set; }

  [StringLength(100)]
  public string? Name { get; set; }


}
