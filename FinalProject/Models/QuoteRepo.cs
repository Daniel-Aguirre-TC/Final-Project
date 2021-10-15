using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace FinalProject.Models
{
    public class QuoteRepo : IQuoteRepo
    {

        private readonly IDbConnection _conn;

        public QuoteRepo(IDbConnection conn)
        {
            _conn = conn;
        }


        public IEnumerable<Comment> GetAllComments()
        {
            return _conn.Query<Comment>("SELECT * FROM comments");
        }

        public IEnumerable<Comment> GetAllComments(int quoteID)
        {
            return _conn.Query<Comment>("SELECT * FROM comments WHERE QuoteID = @QuoteID",
                new { quoteID });
        }

        public IEnumerable<Quote> GetAllQuotes()
        {
            return _conn.Query<Quote>("SELECT * FROM quotes");
        }

        public Quote GetQuote(int quoteID)
        {
            return _conn.QuerySingle<Quote>("SELECT * FROM quotes WHERE QuoteID = @QuoteID;", new { quoteID });
        }

        public void InsertComment(Comment commentToInsert)
        {
            _conn.Execute("INSERT INTO comments (CommentDate, QuoteID, Author, Content) VALUES (CURRENT_TIMESTAMP, @QuoteID, @Author, @Content);",
                new { QuoteID = commentToInsert.QuoteID, commentToInsert.Author, commentToInsert.Content });
        }

        public void InsertQuote(Quote quoteToInsert)
        {

            _conn.Execute("INSERT INTO quotes (Date, CategoryID, Content, Quotee) VALUES (@Date, @Category, @Content, @Quotee);",
                new {quoteToInsert.Date, quoteToInsert.Category, quoteToInsert.Content, quoteToInsert.Quotee });

        }
    }
}
