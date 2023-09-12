using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace Common.Models.Client
{
    public class ClientEmployee
    {
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
        public DateTime? Birthday { get; set; }
        [DisplayName("Пол")]
        public Sex? Sex { get; set; }
        [DisplayName("Дети")]
        public bool? HaveChildren { get; set; }
    }
}
