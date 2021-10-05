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

        public IEnumerable<Quote> GetAllQuotes()
        {
            return _conn.Query<Quote>("SELECT * FROM quotes");
        }
    }
}
