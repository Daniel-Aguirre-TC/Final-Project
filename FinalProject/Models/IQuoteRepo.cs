using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public interface IQuoteRepo
    {

        #region Create/Insert/Post Operations

            /// <summary>Insert the provided Comment into the Database.</summary>
            public void InsertComment(Comment commentToInsert);

            /// <summary>Insert the provided Quote into the Database.</summary>
            public void InsertQuote(Quote quoteToInsert);

        #endregion

        #region Read/Select/Get Operations

            #region Single Quote Returns

                /// <returns>Single Quote from Database based on QuoteID</returns>
                public Quote GetQuote(int quoteID);

                /// <returns>Most recent Quote found in the Database</returns>
                public Quote GetMostRecentQuote();

            #endregion

            #region IEnumerble Quote Returns

                /// <returns>IEnumerable of Quote representing the last seven Quotes in the database.</returns>
                public IEnumerable<Quote> GetThisWeeksQuotes();

                /// <returns>All Quotes from Database</returns>
                public IEnumerable<Quote> GetAllQuotes();

            #endregion

            #region IEnumerable Comments Returns

                /// <returns>IEnumerable of Comment representing all comments in the Database that match the provided QuoteID</returns>
                public IEnumerable<Comment> GetAllComments();

                /// <returns>Single Comment representing all comments in the Database that match the provided QuoteID</returns>
                public IEnumerable<Comment> GetCommentsById(int quoteId);

            #endregion

        #endregion

        // Delete Operation

        /// <summary>Delete the Quote provided from Database based on QuoteID</summary>
        public void DeleteQuote(Quote quoteToDelete);

        // Helper Method 

        /// <returns>True if most recent Quote's date matches todays date. Else returns false.</returns>
        public bool TodaysQuoteExists();

    }
}
