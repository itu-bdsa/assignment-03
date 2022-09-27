using Microsoft.EntityFrameworkCore;

var factory = new KanbanContextFactory();
using var context = factory.CreateDbContext(args);