using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperMarioForum.Data;
using SuperMarioForum.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SuperMarioForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly SuperMarioForumContext _context;

        public HomeController(SuperMarioForumContext context)
        {
            _context = context;
        }

        // GET: Home/Index
        public async Task<IActionResult> Index()
        {
            var discussions = await _context.Discussion
        .Include(d => d.ApplicationUser)  // Include the ApplicationUser to get the owner's name
        .Include(d => d.Comments)        // Include comments to count them
        .OrderByDescending(d => d.CreateDate)
        .ToListAsync();

            return View(discussions);
        }

        // GET: Home/GetDiscussion/{id}
        public async Task<IActionResult> GetDiscussion(int id)
        {
            var discussion = await _context.Discussion
                .Include(d => d.Comments)
                .FirstOrDefaultAsync(d => d.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // GET: Home/Profile?userId=5
        // Define the route directly in the action, avoiding ambiguity
        [HttpGet("Home/Profile/{userId}")]
        public async Task<IActionResult> Profile(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            // Retrieve the user with the specified userId
            var user = await _context.Users
                .Include(u => u.Discussions)  // Optionally include discussions or other data
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}


