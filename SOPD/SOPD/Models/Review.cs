using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SOPD.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public string Content { get; set; }
        [ForeignKey("Thesis")]
        public int ThesisID { get; set; }
        [ForeignKey("Author")]
        public string UserID { get; set; }

        public virtual Thesis Thesis { get; set; }
        public virtual User Author { get; set; }

    }
}