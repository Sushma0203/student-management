using Microsoft.AspNetCore.Mvc;
using MyFirstApp.Data;
using MyFirstApp.Models;
using Microsoft.AspNetCore.Http;

namespace MyFirstApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Students.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student, IFormFile photo)
        {
            if (photo != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                student.Photo = fileName;
            }

            _context.Students.Add(student);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Student Added!";
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            return View(_context.Students.Find(id));
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Update(student);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Student updated!";
                return RedirectToAction("Index");
            }
            return View(student);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var student = _context.Students.Find(id);

            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Student deleted!";
            }

            return RedirectToAction("Index");
        }
    }
}
