using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.DAL
{
    public partial class Country
    {
        public Country()
        {
            this.States = new HashSet<State>();
            this.Students = new HashSet<Students>();
        }

        [Key]
        public int CountryId { get; set; }
        [DisplayName("Country")]
        public string CountryName { get; set; }

        public virtual ICollection<State> States { get; set; }
        public virtual ICollection<Students> Students { get; set; }
    }
}