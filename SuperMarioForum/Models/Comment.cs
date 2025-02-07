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

        // Foreign Key
        [ForeignKey("Discussion")]
        public int DiscussionId { get; set; }

        // Navigation Property - Each comment belongs to one discussion
        public Discussion? Discussion { get; set; }
    }
}
