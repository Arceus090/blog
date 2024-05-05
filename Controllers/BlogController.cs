using BisleriumBlog.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BisleriumBlog.DTO.Request;
using System.Security.Claims;
using BisleriumBlog.DTO;
using BisleriumBlog.Models;

namespace BisleriumBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        public readonly IBlogService _blogService;

        public BlogController(IBlogService postService)
        {
            _blogService = postService;
        }

        [HttpPost, Route("blog/add")]
        [Authorize]
        
        public async Task<IActionResult> AddPost([FromBody] BlogRequestModel model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                var post = await _blogService.AddBlog(userId, model.Title, model.Content, model.ImageUrl);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet, Route("blog/get-all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPost()
        {
            return Ok(await _blogService.GetAllBlogs());
        }

        [HttpGet, Route("post/get-by-id")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetPostByID(string id)
        {
            var result = await _blogService.GetBlogbyId(id);
            return Ok(result);
        }

        [HttpDelete, Route("blog/delete")]
        [Authorize]
      
        public async Task<IActionResult> DeletePostByID(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var posts = await _blogService.GetBlogbyId(id);
            var post = posts.FirstOrDefault(); // Get the first post in case of multiple results

            if (post == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Admin") || post.UserId == userId)
            {
                await _blogService.DeleteBlog(id);
                return Ok();
            }

            return Forbid("Sorry, you are not authorized to delete this resource.");
        }

        [HttpPut, Route("blog/update")]
        [Authorize]
        public async Task<IActionResult> UpdatePostByID(Guid postId, [FromBody] BlogRequestModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve the post by ID from the database
            var existingPost = await _blogService.GetBlogbyId(postId.ToString());

            if (existingPost == null || !existingPost.Any())
            {
                return NotFound();
            }

            // Get the first post from the list
            var post = existingPost.First();

            // Check if the user is authorized to update the post
            if (post.UserId == userId || User.IsInRole("Admin"))
            {
                post.Title = model.Title;
                post.Content = model.Content;
                post.ImageUrl = model.ImageUrl;
                post.UpdatedAt = DateTime.UtcNow;

                // Update the post in the database
                var updatedPost = await _blogService.UpdateBlog(post);

                if (updatedPost != null)
                {
                    return Ok(updatedPost);
                }
                else
                {
                    return StatusCode(500, "Failed to update Blog.");
                }
            }

            // Return Forbidden if user is not authorized to update the post
            return Forbid();
        }
        [HttpGet, Route("blog/history/{id}")]
        [AllowAnonymous] // Adjust authorization as needed
        public async Task<IActionResult> GetBlogHistory(Guid id)
        {
            try
            {
                var blogHistory = await _blogService.GetBlogHistory(id);

                if (blogHistory == null || !blogHistory.Any())
                {
                    return NotFound();
                }

                return Ok(blogHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
