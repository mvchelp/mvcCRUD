using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcDemo.Models
{

    [MustBeTrueAttribute(ErrorMessage = "Please Select Course")]
    public class StudentCoursesViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public bool IsSelected { get; set; }
    }
}