using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Assignment3.Entities;

public partial class KanbanContext : DbContext
{

    public KanbanContext(DbContextOptions<KanbanContext> options)
            : base(options)
        {
        }

    public virtual DbSet<Task> Tasks { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Tag> Tags { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

            });

            modelBuilder.Entity<WorkItem>(entity => {
                entity.Property(e => e.Title).HasMaxLength(50);
                entity.Property(e => e.state).HasConversion(new EnumToStringConverter<State>());
                entity.HasMany<Tag>(s => s.Tags)
                .WithMany(c => c.WorkItems);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


}