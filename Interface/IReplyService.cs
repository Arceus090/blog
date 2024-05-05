using BisleriumBlog.Models;

namespace BisleriumBlog.Interface
{
    public interface IReplyService
    {
        Task<Reply> AddReply(string userId, Guid commentId, string replyText);
        Task<IEnumerable<Reply>> GetAllReplies();
        Task<IEnumerable<Reply>> GetRepliesByCommentId(string commentId);
        Task DeleteReply(string replyId);
        Task<Reply?> UpdateReply(Reply reply);
    }
}
