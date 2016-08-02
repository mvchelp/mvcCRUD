using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.DAL
{
    public partial class State
    {
        public State()
        {
            this.Cities = new HashSet<City>();
            this.Students = new HashSet<Students>();
        }

        [Key]
        public int StateId { get; set; }
        [DisplayName("State")]
        public string StateName { get; set; }
        public Nullable<int> CountryId { get; set; }

        public virtual ICollection<City> Cities { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<Students> Students { get; set; }
    }
}