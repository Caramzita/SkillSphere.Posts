using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Posts.Core.Interfaces;
using SkillSphere.Posts.Core.Models;

namespace SkillSphere.Posts.UseCases.Posts.Queries.GetPostByIdQuery;

public record GetPostByIdQuery(Guid PostId) : IRequest<Result<Post>>;

public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, Result<Post>>
{
    private readonly IPostRepository _postRepository;

    public GetPostByIdQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
    }

    public async Task<Result<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetPostById(request.PostId);

        if (post == null)
        {
            return Result<Post>.Invalid("Post was not found");
        }

        return Result<Post>.Success(post);
    }
}
