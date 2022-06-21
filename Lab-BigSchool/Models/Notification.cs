using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lab_BigSchool.Models
{
    public class NotificationType
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime OriginalDate { get; set; }
        public string OriginalPlace { get; set; }
        public int Type { get; set; }
        public int Course_Id { get; set; }
    }
}