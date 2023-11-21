using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WorkflowEngine;
using TaskStatus = WorkflowEngine.TaskStatus;

namespace dummy
{
    public class TableSplittingContext : DbContext
    {
        public DbSet<EngineTask> EngineTasks { get; set; }
        public DbSet<EcommTask> EcommTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSqlServer(@"Server=.\SQLEXPRESS;Database=EFTableSplitting;Encrypt=False;Trusted_Connection=True;TrustServerCertificate=True")
                .EnableSensitiveDataLogging()
                .LogTo(m => Debug.WriteLine(m));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region TableSplitting
            // modelBuilder.Entity<EcommTask>(
            //     dob =>
            //     {
            //         dob.ToTable("Task");
            //         dob.HasOne(o => o.EngineTask).WithOne()
            //             .HasPrincipalKey<EngineTask>(o => o.Id);
            //         dob.Navigation(o => o.EngineTask).IsRequired();
            //     });
            
            modelBuilder.Entity(typeof(EcommTask), dob =>
            {
                dob.ToTable("Task");
                dob.Property(typeof(TaskStatus?), "Status").HasColumnName("Status");
                dob.HasOne(typeof(EngineTask), "EngineTask")
                    .WithOne()
                    .HasPrincipalKey(typeof(EngineTask), "Id");
                dob.Navigation("EngineTask").IsRequired();
            });

            modelBuilder.Entity<EngineTask>(
                ob =>
                {
                    ob.ToTable("Task");
                    ob.Property(o => o.Status).HasColumnName("Status");
                });
            #endregion
        }
    }
}