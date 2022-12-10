using Employees.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Employees.API.Data
{
    public class EmployeesDbContext : DbContext
    {
        public EmployeesDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees {get;set;}
    }
}