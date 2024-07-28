using ForWhile.Domain;
using ForWhile.Domain.Entities;
using ForWhile.Domain.Enums;
using ForWhile.Domain.Repository.Interface;
using ForWhile.ViewModels.Requests.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ForWhile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly ApplicationDbContext _dbContext;

        public CommentController(ApplicationDbContext dbContext, IRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
            _dbContext = dbContext;
        }

        // GET: api/Comment
        [HttpGet]
        public async Task<ActionResult> GetComments([FromQuery] CommentsGetRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Expression<Func<Comment, object>> orderBy = request.OrderBy switch
                {
                    OrderType.MostVotes => p => p.Upvotes.Count(),
                    OrderType.Recent => p => p.CreatedAt,
                    _ => p => p.Upvotes.Count()
                };

                var comments = await _commentRepository.GetAllAsync(
                    c => c.PostId == request.PostId,
                    orderBy,
                    request.SortDirection,
                    request.PageNumber,
                    request.PageSize,
                    c => c.Author
                    );

                var result = comments.Items.Select(c => new
                {
                    Id = c.Id,
                    Content = c.Content,
                    Author = c.Author.UserName,
                    //TODO avatar ??
                    PostId = c.PostId,
                    Upvote = c.Upvotes.Count(),
                    CreatedAt = c.CreatedAt.ToString("o"),
                });

                return Ok(new { comments = result, totalPages = comments.TotalPages });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
                throw;
            }
        }

        // POST: api/Comment
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment([FromBody] CommentCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var comment = new Comment
                {
                    Content = request.Content,
                    PostId = request.PostID,
                    AuthorId = request.AuthorID,
                };

                await _commentRepository.AddAsync(comment);

                return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
                throw;
            }
        }

        // PUT: api/Comment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, [FromBody] CommentUpdateRequest request)
        {
            if (id != request.Id)
                return BadRequest("Comment ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment is null)
                return NotFound();

            comment.Content = request.Content;

            _dbContext.Entry(comment).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "A concurrency error occurred.");
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Comment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            await _commentRepository.RemoveAsync(comment);

            return NoContent();
        }
    }
}
