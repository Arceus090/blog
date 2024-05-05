using System.ComponentModel.DataAnnotations;

namespace BisleriumBlog.Models
{
    public class Reaction
    {
        [Key]
        public Guid ReactionId { get; set; }

        [Required]
        public string? UserId { get; set; }

        public Guid? BlogId { get; set; }

        public Guid? CommentId { get; set; }

        public Guid? ReplyId { get; set; }

        [Required]
        public ReactionType ReactionType { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ApplicationUser? User { get; set; }

        public virtual Blog? Blog { get; set; }

        public virtual Comment? Comment { get; set; }

        public virtual Reply? Reply { get; set; }
    }

    public enum ReactionType
    {
        Upvote = 1,
        Downvote = 0
    }
}

