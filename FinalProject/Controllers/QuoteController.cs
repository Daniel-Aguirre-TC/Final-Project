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

        public QuoteController(IQuoteRepo quoteRepo)
        {
            repo = quoteRepo;
        }

        public IActionResult Index()
        {
            var thisWeeksQuotes = repo.GetAllQuotes().OrderBy(x => x.Date).Take(7).ToList();
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
