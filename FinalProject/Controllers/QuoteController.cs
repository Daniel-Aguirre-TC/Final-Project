using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class QuoteController : Controller
    {

        private readonly IQuoteRepo repo;
        private readonly QuoteGenerator QuoteAPI;

        public QuoteController(IQuoteRepo quoteRepo)
        {
            repo = quoteRepo;
            QuoteAPI = new QuoteGenerator();
        }

        /// <summary>
        /// If Todays quote does not exist, generate a new one from API. Then Redirect to view this weeks quotes.
        /// </summary>
        public IActionResult Index()
        {
            CheckTodaysQuoteExists();
            return View(repo.GetThisWeeksQuotes());
        }

        /// <summary>
        /// If todays quote does not exist, generate new one from API. Then Redirect to view todays quote using most recent quote.
        /// </summary>
        public IActionResult ViewTodaysQuote()
        {
            CheckTodaysQuoteExists();
            return RedirectToAction("ViewQuote", new { id = repo.GetMostRecentQuote().QuoteID });
        }
      
        /// <summary>
        /// Direct to view the page for the specified QuoteID
        /// </summary>
        public IActionResult ViewQuote(int id)
        {
            var quote = repo.GetQuote(id);
            quote.Comments = repo.GetCommentsById(id).ToList();
            return View(quote);
        }

        /// <summary>
        /// If the QuoteID provided exists then view page to insert comment for that page, otherwise view QuoteNotFound page.
        /// </summary>
        public IActionResult InsertComment(int id)
        {
            var comment = new Comment() {
                QuoteID = id,
                ParentQuote = repo.GetQuote(id)        
            };            
            return repo.GetQuote(id) == null ? View("QuoteNotFound") : View("InsertComment", comment);
        }

        /// <summary>
        /// Insert the provided comment to Database then redirect to ViewQuote page for the QuoteID of the comment.
        /// </summary>
        public IActionResult InsertCommentToDatabase(Comment commentToInsert)
        {
            repo.InsertComment(commentToInsert);
            return RedirectToAction("ViewQuote", new { id = commentToInsert.QuoteID });
        }

        /// <summary>
        /// If todays quote exists then remove it from the database. Then return to HomeController.Index()
        /// </summary>
        public IActionResult DeleteTodaysQuote()
        {
            if (repo.TodaysQuoteExists()) repo.DeleteQuote(repo.GetMostRecentQuote());           
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// If today's quote does not exist then generate a new quote.
        /// </summary>
        void CheckTodaysQuoteExists()
        {
            if (!repo.TodaysQuoteExists())
            {
                // Insert New Quote Pulled from API
                repo.InsertQuote(QuoteAPI.Generate());
            }
        }

    }
}
