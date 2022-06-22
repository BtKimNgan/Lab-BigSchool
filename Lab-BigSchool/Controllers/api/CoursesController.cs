using Lab_BigSchool.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Web.Http;

namespace Lab_BigSchool.Controllers.api
{
    public class CoursesController : ApiController
    {
        public ApplicationDbContext _dbContext { get; set; }
        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();
        }
        [System.Web.Http.HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var course = _dbContext.courses.Single(c => c.Id == id && c.LecturerId == userId);
            if (course.IsCanceled)

                return NotFound();

            course.IsCanceled = true;

            var notification = new Notification()
            {
                DateTime = DateTime.Now,
                Course = course ,
                Type = NotificationType.CourseCanceled
            };
            var attendees = _dbContext.Attendances
                .Where(a => a.CourseId == course.Id)
                .Select(a => a.Attendee)
                .ToList();
            foreach (var attendee in attendees)
            {
                var userNotification = new UserNotification()
                {
                    User = attendee,
                    Notification = notification
                };
                _dbContext.UserNotifications.Add(userNotification);
            }

            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
