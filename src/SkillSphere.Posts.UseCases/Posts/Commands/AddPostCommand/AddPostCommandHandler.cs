﻿using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Posts.Core.Interfaces;
using SkillSphere.Posts.Core.Models;
using SkillSphere.Posts.UseCases.Services;

namespace SkillSphere.Posts.UseCases.Posts.Commands.AddPostCommand;

public class AddPostCommandHandler : IRequestHandler<AddPostCommand, Result<Post>>
{
    private readonly IPostRepository _postRepository;

    private readonly UserProfileServiceClient _userProfileServiceClient;

    public AddPostCommandHandler(IPostRepository postRepository, 
        UserProfileServiceClient userProfileServiceClient)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _userProfileServiceClient = userProfileServiceClient ?? throw new ArgumentNullException(nameof(userProfileServiceClient));
    }

    public async Task<Result<Post>> Handle(AddPostCommand request, CancellationToken cancellationToken)
    {
        if (request.GoalId.HasValue)
        {
            var goal = await _userProfileServiceClient.GetGoalByIdAsync(request.GoalId.Value);

            if (goal == null)
            {
                return Result<Post>.Invalid("Goal not found.");
            }
        }

        if (request.SkillId.HasValue)
        {
            var skill = await _userProfileServiceClient.GetSkillByIdAsync(request.SkillId.Value);

            if (skill == null)
            {
                return Result<Post>.Invalid("Skill not found.");
            }
        };

        var post = new Post(request.UserId, 
            request.Content, 
            request.Type, 
            request.GoalId, 
            request.SkillId);

        await _postRepository.CreatePost(post);

        return Result<Post>.Success(post);
    }
}
