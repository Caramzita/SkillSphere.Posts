using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSphere.Infrastructure.Security.UserAccessor;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Posts.Contracts.DTOs;
using SkillSphere.Posts.Core.Enums;
using SkillSphere.Posts.Core.Models;
using SkillSphere.Posts.UseCases.Posts.Commands.AddPost;
using SkillSphere.Posts.UseCases.Posts.Commands.DeletePost;
using SkillSphere.Posts.UseCases.Posts.Commands.UpdatePost;
using SkillSphere.Posts.UseCases.Posts.Queries.GetAllPosts;
using SkillSphere.Posts.UseCases.Posts.Queries.GetAllUserPostsQuery;
using SkillSphere.Posts.UseCases.Posts.Queries.GetPostByIdQuery;
using SkillSphere.Posts.UseCases.Posts.Queries.GetPostsByGoalIdQuery;
using SkillSphere.Posts.UseCases.Posts.Queries.GetPostsByPostType;
using SkillSphere.Posts.UseCases.Posts.Queries.GetPostsBySkillIdQuery;

namespace SkillSphere.Posts.API.Controllers;

/// <summary>
/// Предоставляет Rest API для работы с постами.
/// </summary>
[Route("api/posts")]
[ApiController]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IMapper _mapper;

    private readonly IMediator _mediator;

    private readonly IUserAccessor _userAccessor;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="PostsController"/>.
    /// </summary>
    /// <param name="mediator"> Интерфейс для отправки команд и запросов через Mediator. </param>
    /// <param name="mapper"> Интерфейс для маппинга данных между моделями. </param>
    /// <param name="userAccessor"> Интерфейс для получения идентификатора пользователя из токена. </param>
    /// <exception cref="ArgumentNullException"> Ошибка загрузки интерфейса. </exception>
    public PostsController(IMapper mapper, IMediator mediator, IUserAccessor userAccessor)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
    }

    /// <summary>
    /// Получить все посты с возможностью сортировки по дате создания.
    /// </summary>
    /// <param name="orderByDescending">
    /// Определяет порядок сортировки постов:
    /// true — сортировка по убыванию (от новых к старым, по умолчанию),
    /// false — сортировка по возрастанию (от старых к новым).
    /// </param>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Post>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public IAsyncEnumerable<Post> GetAllPosts([FromQuery] bool orderByDescending = true)
    {
        var query = new GetAllPostsQuery(orderByDescending);

        return _mediator.CreateStream(query);
    }

    /// <summary>
    /// Получить все посты пользователя с возможностью сортировки по дате создания.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="orderByDescending">
    /// Определяет порядок сортировки постов:
    /// true — сортировка по убыванию (от новых к старым, по умолчанию),
    /// false — сортировка по возрастанию (от старых к новым).
    /// </param>
    [HttpGet("user/{userId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Post>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public IAsyncEnumerable<Post> GetAllUserPosts(Guid userId, 
        [FromQuery] bool orderByDescending = true)
    {
        var query = new GetAllUserPostsQuery(userId, orderByDescending);

        return _mediator.CreateStream(query);
    }

    /// <summary>
    /// Получить пост по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор поста. </param>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Post), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> GetPostById(Guid id)
    {
        var query = new GetPostByIdQuery(id);
        var result = await _mediator.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Получить все посты с заданной целью 
    /// и с возможностью сортировки по дате создания.
    /// </summary>
    /// <param name="goalId"> Идентификатор цели. </param>
    /// <param name="orderByDescending">
    /// Определяет порядок сортировки постов:
    /// true — сортировка по убыванию (от новых к старым, по умолчанию),
    /// false — сортировка по возрастанию (от старых к новым).
    /// </param>
    [HttpGet("goal/{goalId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Post>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public IAsyncEnumerable<Post> GetPostsByGoalId(Guid goalId, 
        [FromQuery] bool orderByDescending = true)
    {
        var query = new GetPostsByGoalIdQuery(goalId, orderByDescending);

        return _mediator.CreateStream(query);
    }

    /// <summary>
    /// Получить все посты связанные с навыками
    /// и с возможностью сортировки по дате создания.
    /// </summary>
    /// <param name="skillIds"> Список идентификаторов навыков. </param>
    /// <param name="orderByDescending">
    /// Определяет порядок сортировки постов:
    /// true — сортировка по убыванию (от новых к старым, по умолчанию),
    /// false — сортировка по возрастанию (от старых к новым).
    /// </param>
    [HttpGet("skills")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Post>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public IAsyncEnumerable<Post> GetPostBySkillIds(
        [FromQuery] List<Guid> skillIds, 
        [FromQuery] bool orderByDescending = true)
    {
        var query = new GetPostsBySkillIdsQuery(skillIds, orderByDescending);

        return _mediator.CreateStream(query);
    }

    /// <summary>
    /// Получить все посты с заданным типом постов
    /// и с возможностью сортировки по дате создания.
    /// </summary>
    /// <param name="postType"> Тип поста. </param>
    /// <param name="orderByDescending">
    /// Определяет порядок сортировки постов:
    /// true — сортировка по убыванию (от новых к старым, по умолчанию),
    /// false — сортировка по возрастанию (от старых к новым).
    /// </param>
    [HttpGet("type/{postType}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Post>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public IAsyncEnumerable<Post> GetPostByPostType(PostType postType, 
        [FromQuery] bool orderByDescending = true)
    {
        var query = new GetPostsByPostType(postType, orderByDescending);

        return _mediator.CreateStream(query);
    }

    /// <summary>
    /// Создать пост.
    /// </summary>
    /// <param name="postDto"> Модель данных поста. </param>
    [HttpPost]
    [ProducesResponseType(typeof(Post), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> CreatePost([FromBody] PostRequestDto postDto)
    {
        var command = _mapper.Map<AddPostCommand>(postDto);
        command.UserId = _userAccessor.GetUserId();

        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Обновить пост.
    /// </summary>
    /// <param name="id"> Идентификатор поста. </param>
    /// <param name="postDto"> Модель данных поста. </param>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(Post), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> UpdatePost(Guid id, [FromBody] PostRequestDto postDto)
    {
        var command = _mapper.Map<UpdatePostCommand>(postDto);
        command.Id = id;
        command.UserId = _userAccessor.GetUserId();

        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить пост.
    /// </summary>
    /// <param name="id"> Идентификатор поста. </param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Unit), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        var userId = _userAccessor.GetUserId();

        var command = new DeletePostCommand(id, userId);
        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }
}