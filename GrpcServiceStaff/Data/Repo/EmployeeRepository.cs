using Common.Models.DB;
using GrpcServiceStaff.Data.DB;
using Microsoft.EntityFrameworkCore;

namespace GrpcServiceStaff.Data.Repo
{
    public class EmployeeRepository : IRepository<Employee>
    {
        private readonly EmployeeDBContext _dbContext;

        public EmployeeRepository(EmployeeDBContext dBContext) { _dbContext = dBContext; }

        public async Task<Employee> Create(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee?> Delete(Employee employee)
        {
            var employeeInTheBase = await _dbContext.Employees.FindAsync(employee.Id);
            if (employeeInTheBase != null)
            {
                var result = _dbContext.Employees.Remove(employeeInTheBase);
                await _dbContext.SaveChangesAsync();
                return result?.Entity;
            }

            return null;
        }

        public void Dispose() { _dbContext.Dispose(); }

        public Task<IQueryable<Employee>> Get() { return Task.FromResult(_dbContext.Employees.AsNoTracking()); }

        public async Task<Employee?> Update(Employee employee)
        {
            var employeeInTheBase = await _dbContext.Employees.FindAsync(employee.Id);
            if (employeeInTheBase != null)
            {
                _dbContext.Entry(employeeInTheBase).CurrentValues.SetValues(employee);
                await _dbContext.SaveChangesAsync();
                return employee;
            }

            return null;
        }
    }
}
