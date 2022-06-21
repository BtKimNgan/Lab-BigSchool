using Lab_BigSchool.DTOs;
using Lab_BigSchool.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Lab_BigSchool.Controllers
{
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _dbContext;
        public AttendancesController()
        {
            _dbContext = new ApplicationDbContext();
        }
        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto attendanceDto)
        {
            var userID = User.Identity.GetUserId();
            if (_dbContext.Attendances.Any(a => a.AttendeeId == userID && a.CourseId == attendanceDto.CourseId))
                return BadRequest(" The Attendance already exists!");
            var attendance = new Attendance
            {
                CourseId = attendanceDto.CourseId,
                AttendeeId =userID
            };

            _dbContext.Attendances.Add(attendance);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
