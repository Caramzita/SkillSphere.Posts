using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Posts.Core.Interfaces;
using SkillSphere.Posts.Core.Models;
using SkillSphere.Posts.UseCases.Services;

namespace SkillSphere.Posts.UseCases.Posts.Commands.UpdatePostCommand;

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

        if (request.GoalId.HasValue)
        {
            var goal = await _userProfileServiceClient.GetGoalByIdAsync(request.GoalId.Value);

            if (goal == null)
            {
                return Result<Post>.Invalid("Goal not found.");
            }

            post.GoalId = goal.Id;
        }

        if (request.SkillId.HasValue)
        {
            var skill = await _userProfileServiceClient.GetSkillByIdAsync(request.SkillId.Value);

            if (skill == null)
            {
                return Result<Post>.Invalid("Skill not found.");
            }

            post.SkillId = skill.Id;
        }

        post.UpdatePost(request.Content, request.Type);
        await _postRepository.UpdatePost(post);

        return Result<Post>.Success(post);
    }
}
