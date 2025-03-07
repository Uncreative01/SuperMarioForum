using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperMarioForum.Data;
using SuperMarioForum.Models;

namespace SuperMarioForum.Controllers
{
    [Authorize]  // Restrict all actions in this controller to authenticated users
    public class DiscussionsController : Controller
    {
        private readonly SuperMarioForumContext _context;

        public DiscussionsController(SuperMarioForumContext context)
        {
            _context = context;
        }

        // GET: Discussions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Discussion.ToListAsync());
        }

        // GET: Discussions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion
                .Include(d => d.Comments)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // GET: Discussions/Create
        [Authorize]  // Ensure only authenticated users can access the create page
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discussions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]  // Ensure only authenticated users can post a new discussion
        public async Task<IActionResult> Create([Bind("DiscussionId,Title,Content")] Discussion discussion, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Set the ApplicationUserId to the logged-in user's ID
                discussion.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Get the logged-in user's ID

                if (imageFile != null && imageFile.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    discussion.ImageFilename = uniqueFileName;
                }

                discussion.CreateDate = DateTime.Now;
                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(discussion);
        }

        // GET: Discussions/Edit/5
        [Authorize]  // Ensure only authenticated users can access the edit page
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion == null)
            {
                return NotFound();
            }
            return View(discussion);
        }

        // POST: Discussions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]  // Ensure only authenticated users can edit the discussion
        public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,CreateDate")] Discussion discussion, IFormFile? imageFile)
        {
            if (id != discussion.DiscussionId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(discussion);
            }

            try
            {
                var existingDiscussion = await _context.Discussion.FindAsync(id);
                if (existingDiscussion == null)
                {
                    return NotFound();
                }

                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Check if a new image was uploaded
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var newImagePath = Path.Combine(uploadDir, uniqueFileName);

                    using (var stream = new FileStream(newImagePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Delete the old image *after* the new one is saved
                    if (!string.IsNullOrEmpty(existingDiscussion.ImageFilename))
                    {
                        var oldImagePath = Path.Combine(uploadDir, existingDiscussion.ImageFilename);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    existingDiscussion.ImageFilename = uniqueFileName;
                }

                existingDiscussion.Title = discussion.Title;
                existingDiscussion.Content = discussion.Content;
                existingDiscussion.CreateDate = discussion.CreateDate;

                _context.Update(existingDiscussion);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error saving changes: " + ex.Message);
                return View(discussion);
            }
        }

        // GET: Discussions/GetDiscussion/5
        public async Task<IActionResult> GetDiscussion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion
                .Include(d => d.Comments) // Include comments for display
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // GET: Discussions/Delete/5
        [Authorize]  // Ensure only authenticated users can access the delete page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // POST: Discussions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]  // Ensure only authenticated users can delete a discussion
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion != null)
            {
                if (!string.IsNullOrEmpty(discussion.ImageFilename))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", discussion.ImageFilename);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Discussion.Remove(discussion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DiscussionExists(int id)
        {
            return _context.Discussion.Any(e => e.DiscussionId == id);
        }
    }
}
