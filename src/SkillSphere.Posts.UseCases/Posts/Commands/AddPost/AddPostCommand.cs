using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Posts.Core.Enums;
using SkillSphere.Posts.Core.Models;

namespace SkillSphere.Posts.UseCases.Posts.Commands.AddPost;

public class AddPostCommand : IRequest<Result<Post>>, IPostCommand
{
    public string Content { get; }

    public PostType Type { get; }

    public Guid UserId { get; set; }

    public Guid? GoalId { get; }

    public List<Guid>? SkillIds { get; }

    public AddPostCommand(string content, PostType type,
        Guid? goalId = null, List<Guid>? skillIds = null)
    {
        Content = content;
        Type = type;
        GoalId = goalId;
        SkillIds = skillIds;
    }
}
