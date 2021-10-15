using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FinalProject.Models
{
    public class QuoteRepo : IQuoteRepo
    {
        private readonly IDbConnection _conn;

        /// <summary>  QuoteRepo Constructor will assign the IDbConnection provided to the private readonly _conn property </summary>
        public QuoteRepo(IDbConnection conn)
        {
            _conn = conn;
        }

        public IEnumerable<Comment> GetAllComments()
        {
            // Return all comments.
            return _conn.Query<Comment>("SELECT * FROM comments");
        }

        public IEnumerable<Comment> GetCommentsById(int quoteID)
        {
            // Return all comments that are tied to the QuoteID provided.
            return _conn.Query<Comment>("SELECT * FROM comments WHERE QuoteID = @QuoteID",
                new { quoteID });
        }

        public IEnumerable<Quote> GetAllQuotes()
        {
            // Return all Quotes from Database
            return _conn.Query<Quote>("SELECT * FROM quotes");     
        }

        public Quote GetQuote(int quoteID)
        {
            // Return the Quote from Database that matches the QuoteID Provided
            return _conn.QuerySingle<Quote>("SELECT * FROM quotes WHERE QuoteID = @QuoteID;", new { quoteID });
        }

        public void InsertComment(Comment commentToInsert)
        {
            // Insert the provided Comment into the Database.
            _conn.Execute("INSERT INTO comments (CommentDate, QuoteID, Author, Content) VALUES (CURRENT_TIMESTAMP, @QuoteID, @Author, @Content);",
                new { QuoteID = commentToInsert.QuoteID, commentToInsert.Author, commentToInsert.Content });
        }

        public void InsertQuote(Quote quoteToInsert)
        {
            // Insert the provided Quote into the Database
            _conn.Execute("INSERT INTO quotes (Date, CategoryID, Content, Quotee) VALUES (@Date, @Category, @Content, @Quotee);",
                new {quoteToInsert.Date, quoteToInsert.Category, quoteToInsert.Content, quoteToInsert.Quotee });
        }

        public bool TodaysQuoteExists()
        {
            // obtain the most recent date from the date column in the quotes table
            var lastCommentDate = _conn.QuerySingle<DateTime>("SELECT DATE(date) from quotes ORDER BY date DESC LIMIT 0, 1").Date;

            // return true if today's mm/dd/yyyy matches the lastCommentDate pulled.
            return DateTime.Now.Date.ToString("MM/dd/yyyy") == lastCommentDate.ToString("MM/dd/yyyy");
        }

        public IEnumerable<Quote> GetThisWeeksQuotes()
        {
            // obtain the seven most recent quotes from database based on Date
            var quotes = _conn.Query<Quote>("SELECT * FROM quotes ORDER BY Date Desc LIMIT 0, 7");
            
            // foreach quote obtained, pull the comments from database and assign to Quote.Comments
            quotes.ToList().ForEach(x => {
                var comment = GetCommentsById(x.QuoteID);
                if (comment != null) x.Comments = comment.ToList();
            });

            return quotes;
        }

        public Quote GetMostRecentQuote()
        {
            // get most recent quote
            var quote = _conn.QuerySingle<Quote>("SELECT * FROM quotes ORDER BY Date Desc LIMIT 0, 1");

            // assign comments for the received quote
            quote.Comments = GetCommentsById(quote.QuoteID).ToList();
        
            return quote;
        }

        public void DeleteQuote(Quote quoteToDelete)
        {
            // Delete comments from Database with that have the matching QuoteID
            _conn.Execute("DELETE FROM comments WHERE QuoteID = @QuoteID",
                    new { quoteToDelete.QuoteID });

            // Delete Quotes from Database that have the matching QuoteID
            _conn.Execute("DELETE FROM quotes WHERE QuoteID = @QuoteID",
                    new { quoteToDelete.QuoteID });
        }
    }
}
