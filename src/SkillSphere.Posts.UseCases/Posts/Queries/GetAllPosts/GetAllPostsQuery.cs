using MediatR;
using SkillSphere.Posts.Core.Interfaces;
using SkillSphere.Posts.Core.Models;
using System.Runtime.CompilerServices;

namespace SkillSphere.Posts.UseCases.Posts.Queries.GetAllPosts;

public record GetAllPostsQuery(bool OrderByDescending) : IStreamRequest<Post>;

public class GetAllPostsQueryHandler : IStreamRequestHandler<GetAllPostsQuery, Post>
{
    private readonly IPostRepository _postRepository;

    public GetAllPostsQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
    }

    public async IAsyncEnumerable<Post> Handle(GetAllPostsQuery request, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var posts = _postRepository.GetAllPosts(request.OrderByDescending);

        await foreach (var post in posts.WithCancellation(cancellationToken))
        {
            yield return post;
        }
    }
}
