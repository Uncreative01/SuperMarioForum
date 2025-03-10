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
    }
}
