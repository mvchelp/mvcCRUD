using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.DAL
{
    public class Course
    {
        public Course()
        {
            this.Students = new HashSet<Students>();
        }

        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public virtual ICollection<Students> Students { get; set; }
    }
}