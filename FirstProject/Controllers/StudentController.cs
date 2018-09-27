using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FirstProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.Controllers
{
    public class StudentController : Controller
    {
        private FprojectContext ORM = null;
        private IHostingEnvironment ENV = null;

        public StudentController(FprojectContext ORM, IHostingEnvironment ENV)
        {
            this.ORM = ORM;
            this.ENV = ENV;
        }
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(Student S,IFormFile CV1)
        {
            String CvPath = ENV.WebRootPath + "/WebData/CVs/";
            String CvName = Guid.NewGuid().ToString();
            String CvExtention = Path.GetExtension(CV1.FileName);
        
            FileStream FS = new FileStream(CvPath + CvName + CvExtention, FileMode.Create);
            CV1.CopyTo(FS);
            FS.Close();

            S.Cv = "/WebData/CVs/" + CvName + CvExtention;
            ORM.Student.Add(S);
            ORM.SaveChanges();

            MailMessage M = new MailMessage();
            M.From = new MailAddress("meharsalman073@gmail.com");
            M.To.Add(new MailAddress(S.Email));
            M.Body = S.Name+ "</br>" + "Thanks for Registration in University of Sialkot" + "Regard : University of Sialkot";
            M.IsBodyHtml = true;
            if (!string.IsNullOrEmpty(S.Cv))
            {
                M.Attachments.Add(new Attachment(ENV.WebRootPath + S.Cv));
            }

            SmtpClient SMTP = new SmtpClient();
            SMTP.Host = "smtp.gmail.com";
            SMTP.Port = 587;
            SMTP.EnableSsl = true;
            SMTP.Credentials = new System.Net.NetworkCredential("meharsalman073@gmail.com", "salman123");
            try
            {
                SMTP.Send(M);
            }
            catch
            {
                ViewBag.Message = "Mail has Sent Successfully";
            }
            ViewBag.Messege = "new Student Added Successfullly";
            return View();
        }
        [HttpGet]
        public IActionResult AllStudent()
        {
            IList<Student> AllStudent = ORM.Student.ToList<Student>();
            return View(AllStudent);
        }
        [HttpPost]
        public IActionResult AllStudent(String SearchByName, String SearchByDept)
        {
            IList<Student> AllStudent = ORM.Student.Where(m => m.Name.Contains(SearchByName) || m.Dept.Contains(SearchByDept)).ToList<Student>();
            return View(AllStudent);
        }
        [HttpGet]
        public IActionResult EditStudent(int Id)
        {
            Student S = ORM.Student.Where(m => m.Id == Id).FirstOrDefault<Student>();
            return View(S);
        }
        [HttpPost]
        public IActionResult EditStudent(Student S)
        {
            ORM.Student.Update(S);
            ORM.SaveChanges();
            return RedirectToAction("AllStudent");
        }
        public IActionResult DetailStudent(int Id)
        {
            Student S = ORM.Student.Where(m => m.Id == Id).FirstOrDefault<Student>();
            return View(S);
        }
        public IActionResult DeleteStudent(Student S)
        {
            ORM.Student.Remove(S);
            ORM.SaveChanges();
            return RedirectToAction("AllStudent");
        }
        public IActionResult AddTeacher()
        {
            return View();
        }
    }
}