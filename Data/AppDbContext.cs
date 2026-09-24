using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<SPM_Role> SPM_Roles => Set<SPM_Role>();
        public DbSet<SPM_User> SPM_Users => Set<SPM_User>();
        public DbSet<SPM_UserType> SPM_UserTypes => Set<SPM_UserType>();
        public DbSet<SPM_UserRole> SPM_UserRoles => Set<SPM_UserRole>();
        public DbSet<SPM_Task> SPM_Tasks => Set<SPM_Task>();
        public DbSet<SPM_TaskStatus> SPM_TaskStatuses => Set<SPM_TaskStatus>();
        public DbSet<SPM_TaskPriority> SPM_TaskPriorities => Set<SPM_TaskPriority>();
        public DbSet<SPM_ProjectMaster> SPM_ProjectMasters => Set<SPM_ProjectMaster>();
        public DbSet<SPM_ProjectAllocation> SPM_ProjectAllocations => Set<SPM_ProjectAllocation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Student Relationship
            modelBuilder.Entity<SPM_ProjectAllocation>()
                .HasOne(pa => pa.Student)
                .WithMany(u => u.StudentProjects)
                .HasForeignKey(pa => pa.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            // Faculty Relationship
            modelBuilder.Entity<SPM_ProjectAllocation>()
                .HasOne(pa => pa.Faculty)
                .WithMany(u => u.FacultyProjects)
                .HasForeignKey(pa => pa.FacultyID)
                .OnDelete(DeleteBehavior.NoAction);

            // Decimal Precision
            modelBuilder.Entity<SPM_Task>()
                .Property(t => t.AssignedScore)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SPM_Task>()
                .Property(t => t.EarnedScore)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SPM_Task>()
                .Property(t => t.ProgressPercentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<SPM_ProjectAllocation>()
                .Property(pa => pa.ProgressPercentage)
                .HasPrecision(5, 2);
        }
    }
}