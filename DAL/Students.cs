using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.DAL
{
    public class Students
    {
        public Students()
        {
            this.Courses = new HashSet<Course>();
        }
    
        [Key]
        public int StudentId { get; set; }
        [Required]
        [DisplayName("First Name")]
        [StringLength(20, ErrorMessage = "Maximum 20 Characters allowed....!")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        [StringLength(20, ErrorMessage = "Maximum 20 Characters allowed....!")]
        public string LastName { get; set; }
        [Required, Display(Name = "Gender")]
        public Nullable<int> GenderId { get; set; }
        [Required, Display(Name = "Country")]
        public Nullable<int> CountryId { get; set; }
        [Required, Display(Name = "State")]
        public Nullable<int> StateId { get; set; }
        [Required, Display(Name = "City")]
        public Nullable<int> CityId { get; set; }
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please Enter Valid Email Address...!")] 
        public string EmailId { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 16 characters....!")]
        public string Password { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
    
        public virtual City City { get; set; }

        public virtual Country Country { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual State State { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}