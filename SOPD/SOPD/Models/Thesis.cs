using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOPD.Models
{
    public class Thesis
    {
        public int ThesisID { get; set; }
        public string Title { get; set; }
        public string EnglishTitle { get; set; }
        public string Abstract { get; set; }
        public string EnglishAbstract { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string KeyWords { get; set; }
        public string EnglishKeyWords { get; set; }
        public ThesisState State { get; set; }
        public int PromoterID { get; set; }
        public int AuthorID { get; set; }   
        public int OrganizationalUnitID { get; set; }
        public int? ReviewerID { get; set; }
        
        public virtual User Promoter { get; set; }
        public virtual User Reviewer { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual OrganizationalUnit OrganizationalUnit { get; set; }
    }

    public enum ThesisState
    {
        Proposition,
        UnApproved,
        Approved,
        Defended,

    }

}