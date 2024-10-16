using SkillSphere.Posts.Core.Enums;

namespace SkillSphere.Posts.Contracts.DTOs;

public class PostRequestDto
{
    public string Content { get; set; } = string.Empty;

    public PostType Type { get; set; }

    public Guid? GoalId { get; set; }

    public List<Guid>? SkillIds { get; set; }
}
