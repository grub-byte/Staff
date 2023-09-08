using Common.Enums;

namespace Common.Models
{
    public class Employee
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public long Birthday { get; set; }
        public Sex Sex { get; set; }
        public bool HaveChildren { get; set; }
    }
}
