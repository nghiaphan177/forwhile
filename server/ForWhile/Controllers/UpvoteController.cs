using ForWhile.Domain;
using ForWhile.Domain.Entities;
using ForWhile.Domain.Enums;
using ForWhile.Domain.Repository.Interface;
using ForWhile.ViewModels.Requests.Upvote;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForWhile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpvoteController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IRepository<Upvote> _upvoteRepository;


        public UpvoteController(ApplicationDbContext dbContext, IRepository<Upvote> upvoteRepository)
        {
            _dbContext = dbContext;
            _upvoteRepository = upvoteRepository;
        }

        // POST: api/Upvote
        [HttpPost]
        public async Task<ActionResult> Upvote([FromBody] UpvoteRequest request)
        {
            try
            {
                var upvote = await _upvoteRepository.GetSingleAsync(uv => uv.UserId == request.UserId
                                                                    && uv.PostId == request.PostId
                                                                    && uv.CommentId == request.CommentId);

                if (upvote is not null)
                {
                    if (upvote.Status.Equals(request.Status))
                        return NoContent();

                    upvote.Status = upvote.Status switch
                    {
                        UpvoteStatus.Upvoted => UpvoteStatus.Neutralized,
                        UpvoteStatus.Downvoted => UpvoteStatus.Neutralized,
                        UpvoteStatus.Neutralized => request.Status,
                        _ => request.Status
                    };

                    _dbContext.Entry(upvote).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    upvote = new Upvote
                    {
                        Status = request.Status,
                        UserId = request.UserId,
                        PostId = request.PostId,
                        CommentId = request.CommentId,
                    };
                    await _upvoteRepository.AddAsync(upvote);
                }

                int upvoteCount;
                if (request.CommentId.HasValue)
                {
                    upvoteCount = (await _upvoteRepository.GetAllAsync(uv => uv.CommentId == request.CommentId
                                                                            && uv.Status == UpvoteStatus.Upvoted)).Count();
                }
                else
                {
                    upvoteCount = (await _upvoteRepository.GetAllAsync(uv => uv.PostId == request.PostId
                                                                            && uv.CommentId == null
                                                                            && uv.Status == UpvoteStatus.Upvoted)).Count();
                }

                return Ok(upvoteCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
                throw;
            }
        }
    }
}
