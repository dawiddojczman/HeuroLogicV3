using HeuroLogicV3.Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HeuroLogicV3.Database.Contexts;

public sealed class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Decision> Decisions => Set<Decision>();
    public DbSet<Criteria> Criterias => Set<Criteria>();
    public DbSet<Option> Options => Set<Option>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Decision>(entity =>
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Title)
                .HasMaxLength(160)
                .IsRequired();

            entity.Property(d => d.AlgorithmVersion)
                .HasMaxLength(60);

            entity.Property(d => d.InputSnapshotJson)
                .HasColumnType("text");

            entity.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.Criterias)
                .WithOne(c => c.Decision)
                .HasForeignKey(c => c.DecisionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.Options)
                .WithOne(o => o.Decision)
                .HasForeignKey(o => o.DecisionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Criteria>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.Name)
                .HasMaxLength(100)
                .IsRequired();
        });
    }
}