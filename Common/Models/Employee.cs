using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace Common.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Фамилия")]
        public string? LastName { get; set; }
        [Required]
        [DisplayName("Имя")]
        public string? FirstName { get; set; }
        [Required]
        [DisplayName("Отчество")]
        public string? MiddleName { get; set; }
        [DisplayName("Дата рождения")]
        public long Birthday { get; set; }
        [DisplayName("Пол")]
        public Sex? Sex { get; set; }
        [DisplayName("Дети")]
        public bool? HaveChildren { get; set; }
    }
}
