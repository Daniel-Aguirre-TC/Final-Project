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

        public IActionResult Index()
        {
            // get seven most recent quotes from database.
            var thisWeeksQuotes = repo.GetAllQuotes().OrderByDescending(x => x.Date).Take(7).ToList();

            //TODO: I can create a repo.GetLatestQuoteDate and simplify my if statement below.
            if (!thisWeeksQuotes.Select(x => x.Date.ToString("MM/dd/yyyy")).Any(d => d == DateTime.Now.ToString("MM/dd/yyyy")))
            {
                // Insert New Quote Pulled from API
                repo.InsertQuote(QuoteAPI.Generate());

            }

            thisWeeksQuotes.ForEach(x => x.Comments = repo.GetAllComments().OrderBy(d => d.CommentDate).ToList());
            return View(thisWeeksQuotes);
        }

        public IActionResult ViewQuote(int id)
        {
            return View(repo.GetQuote(id));
        }

        public IActionResult InsertComment(int id)
        {
            var comment = new Comment() {
                QuoteID = id,
                ParentQuote = repo.GetQuote(id)        
            };
            return repo.GetQuote(id) == null ? View("QuoteNotFound") : View("InsertComment", comment);
        }

        public IActionResult InsertCommentToDatabase(Comment commentToInsert)
        {
            //TODO: I'm getting null data here in InsertCommentToDatabase()
            repo.InsertComment(commentToInsert);
            return RedirectToAction("ViewQuote", new { id = commentToInsert.QuoteID });
        }


    }
}
