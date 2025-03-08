// FeedbackController.cs
using Microsoft.AspNetCore.Mvc;
using System;
using eproject3.Models.Generated;

namespace eproject3.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ABCDMallContext _context;

        public FeedbackController(ABCDMallContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.SubmissionDate = DateTime.Now;
                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();
                return RedirectToAction("ThankYou");
            }
            return View(feedback);
        }
    }
}