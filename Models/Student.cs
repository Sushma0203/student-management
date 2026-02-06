using System.ComponentModel.DataAnnotations;

namespace MyFirstApp.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name required")]
        public required string Name { get; set; }

        [Range(1, 100, ErrorMessage = "Age must be 1–100")]
        public int Age { get; set; }
    }
}
