using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.DAL
{
    public class City
    {
        public City()
        {
            this.Students = new HashSet<Students>();
        }

        [Key]
        public int CityId { get; set; }
        [DisplayName("City")]
        public string CityName { get; set; }
        public Nullable<int> StateId { get; set; }

        public virtual State State { get; set; }
        public virtual ICollection<Students> Students { get; set; }
    }
}