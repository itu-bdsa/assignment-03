namespace Assignment.Infrastructure;

public class WorkItem
{


  public int Id { get; set; }
  [StringLength(100)]
  public string? Title { get; set; }

  public int AssignedTo { get; set; }
  public string? Description { get; set; }

  public State State { get; set; }

  // public

}
public enum State
{
  New, Active, Resolved, Closed, Removed
}
