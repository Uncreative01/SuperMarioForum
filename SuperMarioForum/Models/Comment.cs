using SuperMarioForum.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarioForum.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        // Foreign Key for Discussion
        [ForeignKey("Discussion")]
        public int DiscussionId { get; set; }

        // Navigation Property - Each comment belongs to one discussion
        public virtual Discussion? Discussion { get; set; }

        // Foreign Key for ApplicationUser (nullable to support anonymous comments)
        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }

        // Navigation Property - Each comment may belong to a user
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
