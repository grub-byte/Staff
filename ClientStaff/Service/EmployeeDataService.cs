using System.Linq;
using System.Threading.Tasks;
using Common.Models;

namespace ClientStaff.Service
{
    internal class EmployeeDataService : IDataService<Employee>
    {
        public EmployeeDataService() { }

        public Task<Employee> Create(Employee item)
        {
            throw new System.NotImplementedException();
        }

        public Task<Employee> Delete(Employee item)
        {
            throw new System.NotImplementedException();
        }

        public Task<IQueryable<Employee>> Get()
        {
            throw new System.NotImplementedException();
        }

        public Task<Employee> Update(Employee item)
        {
            throw new System.NotImplementedException();
        }
    }
}
