using System.ComponentModel.DataAnnotations;

namespace MyFirstApp.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Age { get; set; }

        public string? Photo { get; set; }   // NEW
    }
}
