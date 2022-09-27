using Microsoft.EntityFrameworkCore;
namespace Assignment3.Entities;

public partial class KanbanContext : DbContext
{
    public KanbanContext(DbContextOptions<KanbanContext> options)
        : base(options) { }


    public virtual DbSet<User> Users { get; set; }
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Task> Tasks => Set<Task>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>(e =>
        {
            e.Property(c => c.title).IsRequired();
            //TODO: Impelement AssignedTo with optional reference to User entity
            e.Property(c => c.description).HasMaxLength(100);
            e.Property(c => c.state).IsRequired();
            e.Property(c => c.state).HasConversion(
                v => v.ToString(),
                v => (State)Enum.Parse(typeof(State), v));

            //TODO: Implement tag "many-to-many" reference to Tag entity
        });

        modelBuilder.Entity<User>(e =>
        {
            e.Property(c => c.name)
                .IsRequired()
                .HasMaxLength(100);
                
            //TOOD: Make email unique
            e.Property(c => c.email)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Tag>(e =>
        {
            //TODO: Make name unique
            e.Property(c => c.name)
                .IsRequired()
                .HasMaxLength(50);
            //TODO: Implement task "many-to-many" reference to Task entity
        });
    }
}

