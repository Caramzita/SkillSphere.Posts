using MediatR;
using SkillSphere.Posts.Core.Interfaces;
using SkillSphere.Posts.Core.Models;
using SkillSphere.Posts.UseCases.Services;
using System.Runtime.CompilerServices;

namespace SkillSphere.Posts.UseCases.Posts.Queries.GetPostsBySkillIdQuery;

public record GetPostsBySkillIdsQuery(List<Guid> SkillIds, bool OrderByDescending) : IStreamRequest<Post>;

public class GetPostsBySkillIdsQueryHandler : IStreamRequestHandler<GetPostsBySkillIdsQuery, Post>
{
    private readonly IPostRepository _postRepository;

    private readonly UserProfileServiceClient _userProfileServiceClient;

    public GetPostsBySkillIdsQueryHandler(IPostRepository postRepository, UserProfileServiceClient userProfileServiceClient)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _userProfileServiceClient = userProfileServiceClient ?? throw new ArgumentNullException(nameof(userProfileServiceClient));
    }

    public async IAsyncEnumerable<Post> Handle(GetPostsBySkillIdsQuery request, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var skill = await _userProfileServiceClient.GetSkillsByIdsAsync(request.SkillIds);

        if (skill == null)
        {
            yield break;
        }

        var posts = _postRepository.GetPostsBySkillIds(request.SkillIds, request.OrderByDescending);

        await foreach (var post in posts.WithCancellation(cancellationToken))
        {
            yield return post;
        }
    }
}
