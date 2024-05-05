using System.ComponentModel.DataAnnotations;

namespace BisleriumBlog.Models
{
    public class Reply
    {
        [Key]
        public Guid ReplyId { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public Guid CommentId { get; set; }
        [Required]
        public string? ReplyText { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ApplicationUser? User { get; set; }
        public Comment? Comment { get; set; }
        public ICollection<Reaction>? Reactions { get; set; }
    }
}
