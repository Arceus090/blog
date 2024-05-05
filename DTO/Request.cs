using BisleriumBlog.Models;

namespace BisleriumBlog.DTO
{
    public class Request
    {
        public class BlogRequestModel
        {
            public string? Title { get; set; }
            public string? Content { get; set; }
            public string? ImageUrl { get; set; }
        }
        public class CommentRequestModel
        {
            public Guid BlogId { get; set; }
            public Guid CommentId { get; set; }
            public string? CommentText { get; set; }
        }
        public class ReplyRequestModel
        {
            public Guid CommentId { get; set; }
            public string? ReplyText { get; set; }
        }
        public class ReactionRequestModel
        {
            public Guid? BlogId { get; set; }
            public Guid? CommentId { get; set; }
            public Guid? ReplyId { get; set; }
            public ReactionType ReactionType { get; set; }
        }
    }
}
