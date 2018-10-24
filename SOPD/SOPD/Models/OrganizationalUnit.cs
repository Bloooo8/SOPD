using System;
using System.Collections.Generic;

namespace SOPD.Models
{
    public class OrganizationalUnit
    {
        public int OrganizationalUnitID { get; set; }
        public string UnitName { get; set; }

        public virtual ICollection<Thesis> Theses { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}