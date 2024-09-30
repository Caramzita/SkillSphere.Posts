using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Posts.Core.Enums;
using SkillSphere.Posts.Core.Models;

namespace SkillSphere.Posts.UseCases.Posts.Commands.AddPostCommand;

public class AddPostCommand : IRequest<Result<Post>>
{
    public string Content { get; }

    public PostType Type { get; }

    public Guid UserId { get; }

    public Guid? GoalId { get; }

    public Guid? SkillId { get; }

    public AddPostCommand(string content, PostType type, Guid userId, Guid? goalId = null, Guid? skillId = null)
    {
        Content = content;
        UserId = userId;
        Type = type;
        GoalId = goalId;
        SkillId = skillId;
    }
}
