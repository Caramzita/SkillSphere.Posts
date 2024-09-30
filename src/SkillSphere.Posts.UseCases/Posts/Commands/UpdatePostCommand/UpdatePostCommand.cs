using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Posts.Core.Enums;
using SkillSphere.Posts.Core.Models;

namespace SkillSphere.Posts.UseCases.Posts.Commands.UpdatePostCommand;

public class UpdatePostCommand : IRequest<Result<Post>>
{
    public Guid Id { get; }

    public string Content { get; }

    public PostType Type { get; }

    public Guid UserId { get; }

    public Guid? GoalId { get; }

    public Guid? SkillId { get; }

    public UpdatePostCommand(Guid id, string content, PostType type, Guid userId, Guid? goalId = null, Guid? skillId = null)
    {
        Id = id;
        Content = content;
        Type = type;
        UserId = userId;
        GoalId = goalId;
        SkillId = skillId;
    }
}
