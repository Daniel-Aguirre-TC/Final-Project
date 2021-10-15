using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public interface IQuoteRepo
    {
        public IEnumerable<Quote> GetAllQuotes();
        public IEnumerable<Comment> GetAllComments();
        public void InsertComment(Comment commentToInsert);
        public void InsertQuote(Quote quoteToInsert);
        public Quote GetQuote(int quoteID);

    }
}
