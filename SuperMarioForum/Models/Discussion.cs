using SuperMarioForum.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperMarioForum.Models

{
    public class Discussion
    {
        [Key]
        public int DiscussionId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string ImageFilename { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        // Navigation Property - One discussion can have many comments
        public ICollection<Comment>? Comments { get; set; }
    }
}
