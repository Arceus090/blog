using BisleriumBlog.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BisleriumBlog.DTO.Request;
using System.Security.Claims;
using BisleriumBlog.DTO;

namespace BisleriumBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplyController : ControllerBase
    {
        private readonly IReplyService _replyService;
        public ReplyController(IReplyService replyService)
        {
            _replyService = replyService;
        }

        [HttpPost, Route("reply/add")]
        [Authorize]
        public async Task<IActionResult> AddReply([FromBody] ReplyRequestModel model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                var reply = await _replyService.AddReply(userId, model.CommentId, model.ReplyText);
                return Ok(reply);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet, Route("reply/get-all")]
        [Authorize(Roles = UserRoles.Admin)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> GetAllReplies()
        {
            try
            {
                var replies = await _replyService.GetAllReplies();
                return Ok(replies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet, Route("reply/get-by-id")]
        [Authorize(Roles = UserRoles.Admin)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> GetReplyById(string id)
        {
            {
                var replies = await _replyService.GetRepliesByCommentId(id);
                return Ok(replies);
            }
        }

        [HttpDelete, Route("reply/delete")]
        [Authorize(Roles = UserRoles.Admin)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> DeleteReplyById(string id)
        {
            try
            {
                await _replyService.DeleteReply(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut, Route("reply/update")]
        [Authorize(Roles = UserRoles.Admin)]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> UpdateReplyById([FromBody] ReplyRequestModel model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Retrieve the reply by ID from the database
                var existingReplies = await _replyService.GetRepliesByCommentId(model.CommentId.ToString());

                if (existingReplies == null || !existingReplies.Any())
                {
                    return NotFound();
                }

                // Get the first reply from the list
                var reply = existingReplies.First();

                // Check if the user is authorized to update the reply
                if (reply.UserId == userId || User.IsInRole("Admin"))
                {
                    reply.ReplyText = model.ReplyText;
                    reply.UpdatedAt = DateTime.UtcNow;

                    // Update the reply in the database
                    var updatedReply = await _replyService.UpdateReply(reply);

                    if (updatedReply != null)
                    {
                        return Ok(updatedReply);
                    }
                    else
                    {
                        return StatusCode(500, "Failed to update reply.");
                    }
                }
                // Return Forbidden if user is not authorized to update the reply
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
