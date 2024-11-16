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

    public List<Guid>? SkillIds { get; }

    public UpdatePostCommand(string content,
        PostType type, Guid userId, List<Guid>? skillIds = null)
    {
        Content = content;
        Type = type;
        UserId = userId;
        SkillIds = skillIds;
    }
}
