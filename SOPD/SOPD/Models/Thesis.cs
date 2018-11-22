using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SOPD.Models
{
    public class Thesis
    {
        public int ThesisID { get; set; }
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
        [Display(Name = "Angielski tytuł")]
        public string EnglishTitle { get; set; }
        [Display(Name = "Streszczenie")]
        public string Abstract { get; set; }
        [Display(Name = "Angielskie streszczenie")]
        public string EnglishAbstract { get; set; }
        [Display(Name = "Data zatwierdzenia")]
        public DateTime? ApprovalDate { get; set; }
        [Display(Name = "Słowa kluczowe")]
        public string KeyWords { get; set; }
        [Display(Name = "Angielskie słowa kluczowe")]
        public string EnglishKeyWords { get; set; }
        [Display(Name = "Status")]
        public ThesisState State { get; set; }
        [ForeignKey("Promoter")]
        [Display(Name = "Promotor")]
        public string PromoterID { get; set; }
        [ForeignKey("Author")]
        [Display(Name = "Autor")]
        public string AuthorID { get; set; }
        [ForeignKey("OrganizationalUnit")]
        [Display(Name = "Katedra")]
        public int OrganizationalUnitID { get; set; }
        [ForeignKey("Reviewer")]
        [Display(Name = "Recenzent")]
        public string ReviewerID { get; set; }
        
        public virtual User Promoter { get; set; }
        public virtual User Reviewer { get; set; }
        public virtual User Author { get; set; }
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