using Lab_BigSchool.Models;
using Lab_BigSchool.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab_BigSchool.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        private readonly ApplicationDbContext _dbContext;
        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();
        }
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.categories.ToList(),
                Heading = "Add Course"
            };
            return View(viewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.categories.ToList();
                return View("CoursesForm", viewModel);
            }
            var course = new Course
            {
                LecturerId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                CategoryID = viewModel.Category,
                Place = viewModel.Place,
            };
            _dbContext.courses.Add(course);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var courses = _dbContext.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Course)
                .Include(l => l.Lecturer)
                .Include(l => l.category)
                .ToList();

            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated
            };
            return View(viewModel);

        }
        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();

            var courses = _dbContext.courses
                .Where(c => c.LecturerId == userId && c.DateTime > DateTime.Now)

                .Include(l => l.Lecturer)
                .Include(l => l.category)
                .ToList();


            return View(courses);

        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();

            var courses = _dbContext.courses.Single(c => c.Id == id && c.LecturerId == userId);
            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.categories.ToList(),
                Date = courses.DateTime.ToString("dd/M/yyyy"),
                Time = courses.DateTime.ToString("HH:mm"),
                Category = courses.CategoryID,
                Place = courses.Place,
                Heading = "Edit Course",
                Id = courses.Id,

            };
            return View("CoursesForm", viewModel);

        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.categories.ToList();
                return View("CoursesForm", viewModel);
            }
            var userId = User.Identity.GetUserId();

            var courses = _dbContext.courses.Single(c => c.Id == viewModel.Id && c.LecturerId == userId);
            courses.Place = viewModel.Place;
            courses.DateTime = viewModel.GetDateTime();
            courses.CategoryID = viewModel.Category;

            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");

        }

    }
}