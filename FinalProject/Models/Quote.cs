using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Quote
    {
        public int QuoteID { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public string Quotee { get; set; }
        public string Category { get; set; }
        public List<Comment> Comments { get; set; }         
    }


}
