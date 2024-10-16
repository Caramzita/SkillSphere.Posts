using MediatR;
using SkillSphere.Posts.Core.Interfaces;
using SkillSphere.Posts.Core.Models;
using SkillSphere.Posts.UseCases.Services;
using System.Runtime.CompilerServices;

namespace SkillSphere.Posts.UseCases.Posts.Queries.GetPostsByGoalIdQuery;

public record GetPostsByGoalIdQuery(Guid GoalId, bool OrderByDescending) : IStreamRequest<Post>;

public class GetPostsByGoalIdQueryHandler : IStreamRequestHandler<GetPostsByGoalIdQuery, Post>
{
    private readonly IPostRepository _postRepository;

    private readonly UserProfileServiceClient _userProfileServiceClient;

    public GetPostsByGoalIdQueryHandler(IPostRepository postRepository, UserProfileServiceClient userProfileServiceClient)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _userProfileServiceClient = userProfileServiceClient ?? throw new ArgumentNullException(nameof(userProfileServiceClient));
    }

    public async IAsyncEnumerable<Post> Handle(GetPostsByGoalIdQuery request, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var goal = await _userProfileServiceClient.GetGoalByIdAsync(request.GoalId);

        if (goal == null)
        {
            yield break;
        }

        var posts = _postRepository.GetPostsByGoalId(request.GoalId, request.OrderByDescending);

        await foreach (var post in posts.WithCancellation(cancellationToken))
        {
            yield return post;
        }
    }
}