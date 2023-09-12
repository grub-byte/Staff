using Common.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace GrpcServiceStaff.Data.DB
{
    public class EmployeeDBContext : DbContext
    {
        private readonly IConfiguration configuration;
        public DbSet<Employee> Employees { get; set; }

        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("WebApiDatabase"));
        }
    }
}
