using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSphere.Infrastructure.Security.UserAccessor;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Posts.Contracts.DTOs;
using SkillSphere.Posts.Core.Enums;
using SkillSphere.Posts.Core.Models;
using SkillSphere.Posts.UseCases.Posts.Commands.AddPostCommand;
using SkillSphere.Posts.UseCases.Posts.Commands.DeletePostCommand;
using SkillSphere.Posts.UseCases.Posts.Commands.UpdatePostCommand;
using SkillSphere.Posts.UseCases.Posts.Queries.GetAllPosts;
using SkillSphere.Posts.UseCases.Posts.Queries.GetAllUserPostsQuery;
using SkillSphere.Posts.UseCases.Posts.Queries.GetPostByIdQuery;
using SkillSphere.Posts.UseCases.Posts.Queries.GetPostsByGoalIdQuery;
using SkillSphere.Posts.UseCases.Posts.Queries.GetPostsByPostType;
using SkillSphere.Posts.UseCases.Posts.Queries.GetPostsBySkillIdQuery;

namespace SkillSphere.Posts.API.Controllers;

[Route("api/posts")]
[ApiController]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IMapper _mapper;

    private readonly IMediator _mediator;

    private readonly IUserAccessor _userAccessor;

    public PostsController(IMapper mapper, IMediator mediator, IUserAccessor userAccessor)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
    }

    [HttpGet]
    public IAsyncEnumerable<Post> GetAllPosts()
    {
        var query = new GetAllPostsQuery();

        return _mediator.CreateStream(query);
    }

    [HttpGet("user")]
    public IAsyncEnumerable<Post> GetAllUserPosts(CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetUserId();
        var query = new GetAllUserPostsQuery(userId);

        return _mediator.CreateStream(query, cancellationToken);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPostById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetPostByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToActionResult();
    }

    [HttpGet("goal/{goalId:guid}")]
    public IAsyncEnumerable<Post> GetPostByGoalId(Guid goalId)
    {
        var query = new GetPostsByGoalIdQuery(goalId);

        return _mediator.CreateStream(query);
    }

    [HttpGet("skill/{skillId:guid}")]
    public IAsyncEnumerable<Post> GetPostBySkillId(Guid skillId)
    {
        var query = new GetPostsBySkillIdQuery(skillId);

        return _mediator.CreateStream(query);
    }

    [HttpGet("type/{postType}")]
    public IAsyncEnumerable<Post> GetPostBySkillId(PostType postType)
    {
        var query = new GetPostsByPostType(postType);

        return _mediator.CreateStream(query);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostDto postDto)
    {
        var command = _mapper.Map<AddPostCommand>(postDto);
        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePost(Guid id, [FromBody] PostDto postDto)
    {
        var command = _mapper.Map<UpdatePostCommand>(postDto);
        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        var userId = _userAccessor.GetUserId();

        var command = new DeletePostCommand(id, userId);
        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }
}