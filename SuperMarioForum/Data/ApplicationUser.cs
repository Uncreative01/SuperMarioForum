using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarioForum.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string ImageFilename { get; set; } = "default.jpg"; // Default value

        // Mark this property as not mapped to the database
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
