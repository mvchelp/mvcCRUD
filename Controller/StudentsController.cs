using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcDemo.DAL;
using MvcDemo.Models;
using System.Data.Entity.Infrastructure;

namespace MvcDemo.Controllers
{
    public class StudentsController : Controller
    {
        private StudentDBContext db = new StudentDBContext();

        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.City).Include(s => s.Country).Include(s => s.Gender).Include(s => s.State);
            return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.GenderId = new SelectList(db.Gender, "GenderId", "GenderName");
            ViewBag.CountryId = new SelectList(db.Country, "CountryId", "CountryName");
            //ViewBag.StateId = new SelectList(db.State, "StateId", "StateName");
            //ViewBag.CityId = new SelectList(db.City, "CityId", "CityName");

            var student = new Students();
            student.Courses = new List<Course>();
            PopulateCourseData(student);

            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Students students, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                students.Courses = new List<Course>();
                foreach (var course in selectedCourses)
                {
                    var coursesToAdd = db.Course.Find(int.Parse(course));
                    students.Courses.Add(coursesToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                db.Students.Add(students);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Your Data Saved Successfully!!!";
                return RedirectToAction("Index");
            }

            //ViewBag.GenderId = new SelectList(db.Gender, "GenderId", "GenderName", students.GenderId);
            //ViewBag.CountryId = new SelectList(db.Country, "CountryId", "CountryName", students.CountryId);
            //ViewBag.StateId = new SelectList(db.State, "StateId", "StateName", students.StateId);
            //ViewBag.CityId = new SelectList(db.City, "CityId", "CityName", students.CityId);
            return View();
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students student = db.Students
                .Include(i => i.Courses)
                .Where(i => i.StudentId == id).Single();

            if (student == null)
            {
                return HttpNotFound();
            }

            PopulateCourseData(student);

            ViewBag.GenderId = new SelectList(db.Gender, "GenderId", "GenderName", student.GenderId);
            ViewBag.CountryId = new SelectList(db.Country, "CountryId", "CountryName", student.CountryId);
            ViewBag.StateId = new SelectList(db.State.Where(s => s.CountryId == student.CountryId), "StateId", "StateName", student.StateId);
            ViewBag.CityId = new SelectList(db.City.Where(c => c.StateId == student.StateId), "CityId", "CityName", student.CityId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedCourses)
        {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var studentToUpdate = db.Students
                    .Include(i => i.Courses)
                    .Where(i => i.StudentId == id).Single();
                if (TryUpdateModel(studentToUpdate, "",
                   new string[] { "StudentId", "FirstName", "LastName", "GenderId", "CountryId", "StateId", "CityId", "EmailId", "Password", "BirthDate" }))
                {
                    try
                    {
                        UpdateStudentCourses(selectedCourses, studentToUpdate);

                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    catch (RetryLimitExceededException /* dex */)
                    {
                        //Log the error (uncomment dex variable name and add a line here to write a log.
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    }
                }

                PopulateCourseData(studentToUpdate);
                return View(studentToUpdate);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Students students = db.Students.Find(id);
            db.Students.Remove(students);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void PopulateCourseData(Students student)
        {
            var allCourse = db.Course;
            var studentCourse = new HashSet<int>(student.Courses.Select(c => c.CourseId));
            var viewModel = new List<StudentCoursesViewModel>();
            foreach (var course in allCourse)
            {
                viewModel.Add(new StudentCoursesViewModel
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    IsSelected = studentCourse.Contains(course.CourseId)
                });
            }
            ViewBag.Courses = viewModel;
        }

        private void UpdateStudentCourses(string[] selectedCourses, Students studentToUpdate)
        {
            if (selectedCourses == null)
            {
                studentToUpdate.Courses = new List<Course>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var studentCourses = new HashSet<int>
                (studentToUpdate.Courses.Select(c => c.CourseId));
            foreach (var course in db.Course)
            {
                if (selectedCoursesHS.Contains(course.CourseId.ToString()))
                {
                    if (!studentCourses.Contains(course.CourseId))
                    {
                        studentToUpdate.Courses.Add(course);
                    }
                }
                else
                {
                    if (studentCourses.Contains(course.CourseId))
                    {
                        studentToUpdate.Courses.Remove(course);
                    }
                }
            }
        }

        public JsonResult GetStates(string id)
        {
            List<SelectListItem> states = new List<SelectListItem>();
            var stateList = this.Getstate(Convert.ToInt32(id));
            var stateData = stateList.Select(s => new SelectListItem()
            {
                Text = s.StateName,
                Value = s.StateId.ToString(),
            });
            return Json(stateData, JsonRequestBehavior.AllowGet);
        }

        public IList<State> Getstate(int CountryId)
        {
            return db.State.Where(s => s.CountryId == CountryId).ToList();
        }

        public JsonResult GetCities(string id)
        {
            List<SelectListItem> cities = new List<SelectListItem>();
            var cityList = this.Getcity(Convert.ToInt32(id));
            var cityData = cityList.Select(c => new SelectListItem()
            {
                Text = c.CityName,
                Value = c.CityId.ToString(),
            });
            return Json(cityData, JsonRequestBehavior.AllowGet);
        }

        public IList<City> Getcity(int StateId)
        {
            return db.City.Where(c => c.StateId == StateId).ToList();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
