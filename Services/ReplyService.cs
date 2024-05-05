using BisleriumBlog.Interface;
using BisleriumBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace BisleriumBlog.Services
{
    public class ReplyService : IReplyService
    {
        private readonly ApplicationDbContext _dbContext;
        public ReplyService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Reply> AddReply(string userId, Guid commentId, string replyText)
        {
            var reply = new Reply
            {
                ReplyId = Guid.NewGuid(),
                UserId = userId,
                CommentId = commentId,
                ReplyText = replyText,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Replys.Add(reply);
            await _dbContext.SaveChangesAsync();

            return reply;
        }

        public async Task DeleteReply(string replyId)
        {
            var reply = await _dbContext.Replys.FindAsync(Guid.Parse(replyId));
            if (reply != null)
            {
                _dbContext.Replys.Remove(reply);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Reply>> GetAllReplies()
        {
            return await _dbContext.Replys.ToListAsync();
        }

        public async Task<IEnumerable<Reply>> GetRepliesByCommentId(string commentId)
        {
            return await _dbContext.Replys.Where(r => r.ReplyId.ToString() == commentId).ToListAsync();
        }

        public async Task<Reply?> UpdateReply(Reply reply)
        {
            var existingReply = await _dbContext.Replys.FindAsync(reply.ReplyId);
            if (existingReply != null)
            {
                existingReply.ReplyText = reply.ReplyText;
                existingReply.UpdatedAt = DateTime.UtcNow;

                _dbContext.Entry(existingReply).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return existingReply;
            }
            else
            {
                return null;
            }
        }
    }
}
