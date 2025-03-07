using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace SuperMarioForum.Data
{
    public class ApplicationUser : IdentityUser
    {
        // Custom Fields
        public string Name { get; set; }  // Required field
        public string Location { get; set; }  // Optional field
        public string ImageFilename { get; set; }  // Store filename for image
        public IFormFile ImageFile { get; set; }  // File upload (not mapped to database)
    }
}