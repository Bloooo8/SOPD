using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SOPD.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        [Display(Name = "Treść")]
        public string Content { get; set; }
        [ForeignKey("Thesis")]
        public int ThesisID { get; set; }
        [Display(Name = "Autor recenzji")]
        [ForeignKey("Author")]
        public string UserID { get; set; }

        public virtual Thesis Thesis { get; set; }
        public virtual User Author { get; set; }

    }
}