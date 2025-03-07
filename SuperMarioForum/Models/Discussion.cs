using SuperMarioForum.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        // Foreign Key for ApplicationUser
        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }

        // Navigation Property (nullable)
        public virtual ApplicationUser? ApplicationUser { get; set; }

        // Navigation Property - One discussion can have many comments
        public ICollection<Comment>? Comments { get; set; }
    }
}
