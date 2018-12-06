using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SOPD.Models
{
    public class OrganizationalUnit
    {

        public int OrganizationalUnitID { get; set; }
        [Display(Name = "Jednostka organizacyjna")]
        public string UnitName { get; set; }

        public virtual ICollection<Thesis> Theses { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}