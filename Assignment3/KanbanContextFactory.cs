using Assignment3.Entities;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Assignment3
{
    internal class KanbanContextFactory : IDesignTimeDbContextFactory<KanbanContext>
    {
        public KanbanContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddUserSecrets<KanbanContext>().Build();
            var connectionString = configuration.GetConnectionString("Kanban");

            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new KanbanContext(optionsBuilder.Options);
        }
    }
}
