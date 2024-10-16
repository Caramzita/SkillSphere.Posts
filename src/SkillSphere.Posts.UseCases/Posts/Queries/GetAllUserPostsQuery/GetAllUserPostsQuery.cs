using MediatR;
using SkillSphere.Posts.Core.Interfaces;
using SkillSphere.Posts.Core.Models;
using System.Runtime.CompilerServices;

namespace SkillSphere.Posts.UseCases.Posts.Queries.GetAllUserPostsQuery;

public record GetAllUserPostsQuery(Guid UserId, bool OrderByDescending) : IStreamRequest<Post>;

public class GetAllUserPostsQueryHandler : IStreamRequestHandler<GetAllUserPostsQuery, Post>
{
    private readonly IPostRepository _postRepository;

    public GetAllUserPostsQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
    }

    public async IAsyncEnumerable<Post> Handle(GetAllUserPostsQuery request, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var posts = _postRepository.GetAllUserPosts(request.UserId, request.OrderByDescending);

        await foreach (var post in posts.WithCancellation(cancellationToken))
        {
            yield return post;
        }
    }
}
