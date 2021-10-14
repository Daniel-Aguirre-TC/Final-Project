using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public DateTime CommentDate { get; set; }
        // used to obtain ParentQuote when needed.
        public int QuoteID { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }

        
        public Quote ParentQuote { get; set; }

    }


}
