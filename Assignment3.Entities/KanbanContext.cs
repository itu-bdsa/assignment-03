namespace Assignment3.Entities;
using Assignment3.Core;

public class KanbanContext : DbContext, IKanbanContext
{
    public KanbanContext(DbContextOptions<KanbanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Task> Tasks => Set<Task>();
    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Task>()
            .Property(e => e.State)
            .HasConversion(
                v => v.ToString(),
                v => (State)Enum.Parse(typeof(State), v));
    }

}
