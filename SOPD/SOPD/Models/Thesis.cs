﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey("Promoter")]
        public string PromoterID { get; set; }
        [ForeignKey("Author")]
        public string AuthorID { get; set; }
        [ForeignKey("OrganizationalUnit")]
        public int OrganizationalUnitID { get; set; }
        [ForeignKey("Reviewer")]
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