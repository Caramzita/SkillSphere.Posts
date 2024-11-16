using SkillSphere.Posts.Core.Enums;

namespace SkillSphere.Posts.UseCases.Posts.Commands;

public interface IPostCommand
{
    string Content { get; }

    PostType Type { get; }

    List<Guid>? SkillIds { get; }
}
