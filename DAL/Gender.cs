using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.DAL
{
    public class Gender
    {
        public Gender()
        {
            this.Students = new HashSet<Students>();
        }

        [Key]
        public int GenderId { get; set; }
        [DisplayName("Gender")]
        public string GenderName { get; set; }

        public virtual ICollection<Students> Students { get; set; }
    }
}