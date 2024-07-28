using ForWhile.Domain;
using ForWhile.Domain.Entities;
using ForWhile.Domain.Enums;
using ForWhile.Domain.Repository.Interface;
using ForWhile.ViewModels.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ForWhile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<Upvote> _upvoteRepository;
        private readonly IPostTagRepository _postTagRepository;
        private readonly IConfiguration _configuration;

        public PostController(ApplicationDbContext context, IRepository<Post> postRepository, IRepository<Tag> tagRepository,
            IPostTagRepository postTagRepository, IRepository<Upvote> upvoteRepository, IRepository<Comment> commentRepository,
            UserManager<User> userManager, IConfiguration configuration)
        {
            _context = context;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;
            _upvoteRepository = upvoteRepository;
            _postTagRepository = postTagRepository;
            _userManager = userManager;
            _configuration = configuration;
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPost(int id)
        {
            var post = await _postRepository.GetByIdAsync(id, p => p.Author, p => p.PostTags, p => p.Upvotes, p => p.Comments);

            if (post == null)
            {
                return NotFound();
            }

            var result = new
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Author = post.Author.UserName,
                //TODO avatar ??
                CreatedAt = post.CreatedAt.ToString("o"),
                UpdatedAt = post.UpdatedAt.ToString("o"),
                Upvote = post.Upvotes.Count(uv => uv.Status == UpvoteStatus.Upvoted && uv.CommentId is null),
                Tags = post.PostTags?.Select(pt => pt.Tag.Name).ToList(),
                CommentCount = post.Comments.Count,
                TopComments = post.Comments.OrderByDescending(c => c.Upvotes.Count(uv => uv.Status == UpvoteStatus.Upvoted))
                                .Take(10)
                                .Select(c =>
                                {
                                    var author = c.Author;
                                    return new
                                    {
                                        Id = c.Id,
                                        Author = author.UserName,
                                        Content = c.Content,
                                        Upvote = c.Upvotes.Count(uv => uv.Status == UpvoteStatus.Upvoted),
                                        CreatedAt = c.CreatedAt.ToString("o"),
                                        UpdatedAt = c.UpdatedAt.ToString("o")
                                    };
                                })
            };

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] PostsGetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Expression<Func<Post, bool>> predicate = p => p.TypeId == request.TypeId;

            Expression<Func<Post, object>> orderBy = request.OrderBy switch
            {
                OrderType.MostVotes => p => p.Upvotes.Count(x => x.CommentId == null),
                OrderType.Recent => p => p.CreatedAt,
                OrderType.MostView => p => p.View,
                _ => p => p.Upvotes.Count(x => x.CommentId == null)
            };

            IQueryable<Post> query = _postRepository.GetAll()
                                    .Include(p => p.Author)
                                    .Include(p => p.PostTags)
                                        .ThenInclude(pt => pt.Tag)
                                    .Include(p => p.Comments)
                                        .ThenInclude(c => c.Author)
                                    .Include(p => p.Upvotes);

            var pageResult = await _postRepository.GetAllAsync(query, predicate, orderBy,
                                                        request.SortDirection, request.PageNumber, request.PageSize);

            var result = pageResult.Items.Select(p =>
            {

                var lastComment = p.Comments.OrderByDescending(c => c.CreatedAt).FirstOrDefault();
                return new
                {
                    PostId = p.Id,
                    Title = p.Title,
                    CreatedAt = p.CreatedAt.ToString("o"),
                    Author = p.Author.UserName,
                    Vote = p.Upvotes.Count(x => x.CommentId == null),
                    View = p.View,
                    Tags = p.PostTags?.Select(pt => pt.Tag.Name).ToList(),
                    LastCommentAuthor = lastComment?.Author.UserName,
                    LastCommentCreatedAt = lastComment?.CreatedAt.ToString("o"),
                };
            });

            return Ok(new { posts = result, totalPages = pageResult.TotalPages });
        }

        // POST: api/Post
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost([FromBody] PostCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = new Post()
            {
                Title = request.Title,
                Content = request.Content,
                AuthorId = request.AuthorId,
                TypeId = request.TypeId,
                CreatedAt = DateTime.UtcNow,
            };
            await _postRepository.AddAsync(post);

            if (request.Tags != null)
            {
                foreach (var tagName in request.Tags)
                {
                    var tag = await _tagRepository.GetSingleAsync(x => x.Name == tagName);
                    if (tag is null)
                    {
                        tag = new Tag()
                        {
                            Name = tagName,
                        };
                        await _tagRepository.AddAsync(tag);
                    }
                    tag.Count++;

                    var postTag = new PostTag()
                    {
                        PostId = post.Id,
                        TagId = tag.Id,
                    };
                    await _tagRepository.UpdateAsync(tag);
                    await _postTagRepository.AddAsync(postTag);
                }
            }

            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        // PUT: api/Posts/5
        [HttpPut]
        public async Task<IActionResult> PutPost(int id, [FromBody] PostUpdateRequest request)
        {
            if (id != request.Id)
                return BadRequest("Comment ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = await _postRepository.GetByIdAsync(request.Id, x => x.PostTags);

            if (post is null)
                return NotFound();

            post.Title = request.Title;
            post.Content = request.Content;

            if (request.Tags is not null)
            {
                var obsoletePostTags = new List<PostTag>();
                foreach (var postTag in post.PostTags)
                {
                    if (!request.Tags.Contains(postTag.Tag.Name))
                    {
                        obsoletePostTags.Add(postTag);
                    }
                }

                if (obsoletePostTags.Count > 0)
                {
                    await _postTagRepository.RemoveRangeAsync(obsoletePostTags);
                }

                foreach (var tagName in request.Tags)
                {
                    var tag = await _tagRepository.GetSingleAsync(x => x.Name == tagName);
                    if (tag is null)
                    {
                        tag = new Tag()
                        {
                            Name = tagName,
                        };
                        await _tagRepository.AddAsync(tag);
                    }

                    if (!await _postTagRepository.ExistsAsync(post.Id, tag.Id))
                    {
                        tag.Count++;
                        var postTag = new PostTag()
                        {
                            PostId = post.Id,
                            TagId = tag.Id,
                        };
                        await _tagRepository.UpdateAsync(tag);
                        await _postTagRepository.AddAsync(postTag);
                    }
                }
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "A concurrency error occurred.");
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            await _upvoteRepository.RemoveRangeAsync(post.Upvotes);

            await _postRepository.RemoveAsync(post);

            return NoContent();
        }
    }
}
