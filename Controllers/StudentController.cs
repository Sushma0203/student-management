using Microsoft.AspNetCore.Mvc;
using MyFirstApp.Data;
using MyFirstApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;

namespace MyFirstApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            var students = _context.Students.ToList();
            return View(students);
        }

        // ================= CREATE GET =================
        public IActionResult Create()
        {
            return View();
        }

        // ================= CREATE POST =================
        [HttpPost]
        public IActionResult Create(Student student, IFormFile photo)
        {
            if (photo != null && photo.Length > 0)
            {
                string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                string filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                student.Photo = fileName;
            }

            _context.Students.Add(student);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Student added successfully!";
            return RedirectToAction("Index");
        }

        // ================= EDIT GET =================
        public IActionResult Edit(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null) return NotFound();
            return View(student);
        }

        // ================= EDIT POST =================
        [HttpPost]
        public IActionResult Edit(Student student, string CroppedImage)
        {
            var existing = _context.Students.Find(student.Id);
            if (existing == null) return NotFound();

            existing.Name = student.Name;
            existing.Age = student.Age;

            // ===== Update photo if new cropped image is sent =====
            if (!string.IsNullOrEmpty(CroppedImage))
            {
                string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                string fileName = Guid.NewGuid().ToString() + ".jpg";

                // Remove base64 header if exists
                var base64Data = CroppedImage.Contains(",") ? CroppedImage.Split(',')[1] : CroppedImage;
                byte[] bytes = Convert.FromBase64String(base64Data);

                System.IO.File.WriteAllBytes(Path.Combine(uploads, fileName), bytes);

                existing.Photo = fileName;
            }

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Student updated successfully!";
            return RedirectToAction("Index");
        }

        // ================= DELETE =================
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Student deleted successfully!";
            }

            return RedirectToAction("Index");
        }
    }
}
