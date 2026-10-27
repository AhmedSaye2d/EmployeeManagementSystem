using BaseLibrary.Entities;
using Microsoft.EntityFrameworkCore;
namespace HostLibrary.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Employee> Employees { get; set; }

        //GeneralDepartment / Depaertment / Branch
        public DbSet<GeneralDepartment> GeneralDepartments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Branch> Branches { get; set; }
        //Country / City  / Town
        public DbSet<Town> Towns { get; set; }

        //Auth /Role / System Role
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<SystemRole> SystemRoles { get; set; }
        public DbSet<RefreshTokenInfo> RefreshTokenInfos { get; set; }
        //other / overtime / Vacation / sanction 
        public DbSet<OverTime> OverTimes { get; set; }
        public DbSet<OverTimeType> OverTimeTypes { get; set; }
        public DbSet<VacationType> VacationTypes { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<Snaction> Snactions { get; set; }
        public DbSet<SnactionType> SnactionsType { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

    }
}
