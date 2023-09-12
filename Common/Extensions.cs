using Common.Models.Client;
using Common.Models.DB;
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
            Sex = (Common.Enums.Sex)message.Sex,
            Id = message.Id,
        };
        public static ClientEmployee ToClientEmployee(this WorkerMessage message) => new ClientEmployee()
        {
            Birthday = new DateTime(message.Birthday),
            FirstName = message.FirstName,
            HaveChildren = message.HaveChildren,
            LastName = message.LastName,
            MiddleName = message.MiddleName,
            Sex = (Common.Enums.Sex)message.Sex,
            Id = message.Id
        };

        public static WorkerMessage ToWorkerMessage(this Employee employee) => new WorkerMessage()
        {
            Birthday = employee.Birthday,
            FirstName = employee.FirstName,
            HaveChildren = employee.HaveChildren ?? false,
            LastName = employee.LastName,
            MiddleName = employee.MiddleName,
            Sex = employee.Sex.HasValue ? (Sex)employee.Sex : Sex.Default,
            Id = employee.Id,
        };
        public static WorkerMessage ToWorkerMessage(this ClientEmployee employee) => new WorkerMessage()
        {
            Birthday = employee.Birthday.HasValue ? employee.Birthday.Value.Ticks : 0,
            FirstName = employee.FirstName,
            HaveChildren = employee.HaveChildren ?? false,
            LastName = employee.LastName,
            MiddleName = employee.MiddleName,
            Sex = employee.Sex.HasValue ? (Sex)employee.Sex : Sex.Default,
            Id = employee.Id
        };
    }
}
