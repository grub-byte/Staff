using Common.Models;
using GrpcServiceStaff;

namespace Common
{
    public static class Extensions
    {
        public static Employee ToEmployee(this WorkerMessage message) => new Employee()
        {
            Birthday = message.Birthday,
            FirstName = message.FirstName,
            HaveChildren = message.HaveChildren,
            LastName = message.LastName,
            MiddleName = message.MiddleName,
            Sex = (Common.Enums.Sex)message.Sex
        };

        public static WorkerMessage ToWorkerMessage(this Employee employee) => new WorkerMessage()
        {
            Birthday = employee.Birthday,
            FirstName = employee.FirstName,
            HaveChildren = employee.HaveChildren ?? false,
            LastName = employee.LastName,
            MiddleName = employee.MiddleName,
            Sex = employee.Sex.HasValue ? (Sex)employee.Sex : Sex.Default
        };
    }
}
