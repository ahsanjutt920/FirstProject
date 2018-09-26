using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.Controllers
{
    public class StudentController : Controller
    {
        private FprojectContext ORM = null;
        public StudentController(FprojectContext ORM)
        {
            this.ORM = ORM;
        }
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(Student S)
        {
            ORM.Student.Add(S);
            ORM.SaveChanges();
            ViewBag.Messege = "new Student Added Successfullly";
            return View();
        }
        public IActionResult AddTeacher()
        {
            return View();
        }
    }
}