using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Posts.Core.Interfaces;
using SkillSphere.Posts.Core.Models;
using SkillSphere.Posts.UseCases.Services;

namespace SkillSphere.Posts.UseCases.Posts.Commands.UpdatePost;

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Result<Post>>
{
    private readonly IPostRepository _postRepository;

    private readonly UserProfileServiceClient _userProfileServiceClient;

    public UpdatePostCommandHandler(IPostRepository postRepository, UserProfileServiceClient userProfileServiceClient)
    {
        _postRepository = postRepository;
        _userProfileServiceClient = userProfileServiceClient;
    }

    public async Task<Result<Post>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetPostById(request.Id);

        if (post == null)
        {
            return Result<Post>.Invalid("Post not found.");
        }

        if (post.UserId != request.UserId)
        {
            return Result<Post>.Invalid("Error while get post.");
        }

        if (request.GoalId.HasValue)
        {
            var goal = await _userProfileServiceClient.GetGoalByIdAsync(request.GoalId.Value);

            if (goal == null)
            {
                post.GoalId = null;
            }
        }

        if (request.SkillIds != null && request.SkillIds.Count > 0)
        {
            var skills = await _userProfileServiceClient.GetSkillsByIdsAsync(request.SkillIds);
            if (skills == null || skills.Count != request.SkillIds.Count)
            {
                return Result<Post>.Invalid("Some skills were not found.");
            }
        }

        post.UpdatePost(request.Content, request.Type, request.SkillIds);
        await _postRepository.UpdatePost(post);

        return Result<Post>.Success(post);
    }
}