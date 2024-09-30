using MediatR;
using SkillSphere.Posts.Core.Interfaces;
using SkillSphere.Posts.Core.Models;
using SkillSphere.Posts.UseCases.Services;
using System.Runtime.CompilerServices;

namespace SkillSphere.Posts.UseCases.Posts.Queries.GetPostsBySkillIdQuery;

public record GetPostsBySkillIdQuery(Guid SkillId) : IStreamRequest<Post>;

public class GetPostsBySkillIdQueryHandler : IStreamRequestHandler<GetPostsBySkillIdQuery, Post>
{
    private readonly IPostRepository _postRepository;

    private readonly UserProfileServiceClient _userProfileServiceClient;

    public GetPostsBySkillIdQueryHandler(IPostRepository postRepository, UserProfileServiceClient userProfileServiceClient)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _userProfileServiceClient = userProfileServiceClient ?? throw new ArgumentNullException(nameof(userProfileServiceClient));
    }

    public async IAsyncEnumerable<Post> Handle(GetPostsBySkillIdQuery request, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var skill = await _userProfileServiceClient.GetSkillByIdAsync(request.SkillId);

        if (skill == null)
        {
            yield break;
        }

        var posts = _postRepository.GetPostsBySkillId(request.SkillId);

        await foreach (var post in posts.WithCancellation(cancellationToken))
        {
            yield return post;
        }
    }
}
