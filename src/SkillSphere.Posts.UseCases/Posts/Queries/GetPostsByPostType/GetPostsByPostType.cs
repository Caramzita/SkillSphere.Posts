using MediatR;
using SkillSphere.Posts.Core.Enums;
using SkillSphere.Posts.Core.Interfaces;
using SkillSphere.Posts.Core.Models;
using System.Runtime.CompilerServices;

namespace SkillSphere.Posts.UseCases.Posts.Queries.GetPostsByPostType;

public record GetPostsByPostType(PostType Type, bool OrderByDescending) : IStreamRequest<Post>;

public class GetPostsByPostTypeHandler : IStreamRequestHandler<GetPostsByPostType, Post>
{
    private readonly IPostRepository _postRepository;

    public GetPostsByPostTypeHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
    }

    public async IAsyncEnumerable<Post> Handle(GetPostsByPostType request, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var posts = _postRepository.GetPostsByPostType(request.Type, request.OrderByDescending);

        await foreach (var post in posts.WithCancellation(cancellationToken))
        {
            yield return post;
        }
    }
}