using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperMarioForum.Data;
using SuperMarioForum.Models;
using System;
using System.Threading.Tasks;

namespace SuperMarioForum.Controllers
{
    [Authorize]  // Restrict access to all actions in this controller to authenticated users
    public class CommentsController : Controller
    {
        private readonly SuperMarioForumContext _context;

        public CommentsController(SuperMarioForumContext context)
        {
            _context = context;
        }

        // GET: Comments/Create
        [Authorize]  // Ensure only authenticated users can access the create page
        public IActionResult Create(int discussionId)
        {
            var comment = new Comment { DiscussionId = discussionId };
            return View(comment);
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]  // Ensure only authenticated users can post a new comment
        public async Task<IActionResult> Create([Bind("Content,DiscussionId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreateDate = DateTime.Now;
                // Here, you can also assign the UserId to the comment
                comment.ApplicationUserId = User.Identity.Name; // Assuming you have ApplicationUserId as a string property in your Comment model

                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetDiscussion", "Discussions", new { id = comment.DiscussionId });
            }
            return View(comment);
        }
    }
}
