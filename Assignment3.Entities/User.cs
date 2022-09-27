namespace Assignment3.Entities;

public class User {
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public List<Task> tasks { get; set; }
}
