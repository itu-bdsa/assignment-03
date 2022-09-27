namespace Assignment3;

public class KanbanContextFactory : IDesignTimeDbContextFactory<KanbanContext>
{
    public KanbanContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<KanbanContext>().Build();
        var connectionString = configuration.GetConnectionString("ConnectionString");

        String connectionStringHardcode = "Server=localhost;Database=stoic_chandrasekhar;User Id=sa;Password=BDSAagain22;Trusted_Connection=False;Encrypt=False";

        var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
        optionsBuilder.UseSqlServer(connectionStringHardcode);

        return new KanbanContext(optionsBuilder.Options);
    }
}