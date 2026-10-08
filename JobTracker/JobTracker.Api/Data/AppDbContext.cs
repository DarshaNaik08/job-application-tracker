using JobTracker.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace JobTracker.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<Interview> Interviews => Set<Interview>();
    public DbSet<ApplicationNote> Notes => Set<ApplicationNote>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Company>(e =>
        {
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.Website).HasMaxLength(500);
            e.Property(x => x.Location).HasMaxLength(200);
            e.HasIndex(x => x.Name).IsUnique();
        });

        b.Entity<JobApplication>(e =>
        {
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Location).HasMaxLength(200);
            e.Property(x => x.Source).IsRequired().HasMaxLength(100);
            e.Property(x => x.JobUrl).HasMaxLength(1000);

            // Store enum names ("Applied") instead of numbers (1)
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.WorkMode).HasConversion<string>().HasMaxLength(20);

            e.HasIndex(x => x.Status);

            // Deleting a company is blocked while it still has applications
            e.HasOne(x => x.Company)
             .WithMany(c => c.Applications)
             .HasForeignKey(x => x.CompanyId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Interview>(e =>
        {
            e.Property(x => x.Type).IsRequired().HasMaxLength(100);
            e.Property(x => x.Outcome).HasMaxLength(500);

            // Deleting an application deletes its interviews
            e.HasOne(x => x.JobApplication)
             .WithMany(a => a.Interviews)
             .HasForeignKey(x => x.JobApplicationId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<ApplicationNote>(e =>
        {
            e.Property(x => x.Text).IsRequired().HasMaxLength(2000);

            e.HasOne(x => x.JobApplication)
             .WithMany(a => a.Notes)
             .HasForeignKey(x => x.JobApplicationId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}