using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOPD.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public string Content { get; set; }
        public int ThesisID { get; set; }

        public virtual Thesis Thesis { get; set; }

    }
}