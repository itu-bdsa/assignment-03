// See https://aka.ms/new-console-template for more information
var factory = new KanbanContextFactory();
using var context = factory.CreateDbContext(args);
