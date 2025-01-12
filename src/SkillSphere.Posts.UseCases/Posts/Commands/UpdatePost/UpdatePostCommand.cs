using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Posts.Core.Enums;
using SkillSphere.Posts.Core.Models;

namespace SkillSphere.Posts.UseCases.Posts.Commands.UpdatePost;

public class UpdatePostCommand : IRequest<Result<Post>>, IPostCommand
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Content { get; }

    public PostType Type { get; }

    public Guid? GoalId { get; }

    public List<Guid>? SkillIds { get; }

    public UpdatePostCommand(string content, 
        PostType type, List<Guid>? skillIds = null, Guid? goalId = null)
    {
        Content = content;
        Type = type;
        SkillIds = skillIds;
        GoalId = goalId;
    }
}
