using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Posts.Core.Interfaces;

namespace SkillSphere.Posts.UseCases.Posts.Commands.DeletePost;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Result<Unit>>
{
    private readonly IPostRepository _postRepository;

    public DeletePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
    }

    public async Task<Result<Unit>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetPostById(request.Id);

        if (post == null)
        {
            return Result<Unit>.Invalid("Post not found.");
        }

        if (post.UserId != request.UserId)
        {
            return Result<Unit>.Invalid("Error while get post.");
        }

        await _postRepository.DeletePost(post);

        return Result<Unit>.Empty();
    }
}
