using EmployeePortal.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.Data;

public class PortalDbContext : DbContext
{
    public PortalDbContext(DbContextOptions<PortalDbContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Manager)
            .WithMany()
            .HasForeignKey(e => e.ManagerId);

        modelBuilder.Entity<LeaveRequest>()
            .HasOne(lr => lr.Employee)
            .WithMany()
            .HasForeignKey(lr => lr.EmployeeId);
    }
}
