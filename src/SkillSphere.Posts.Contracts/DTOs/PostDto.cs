using SkillSphere.Posts.Core.Enums;

namespace SkillSphere.Posts.Contracts.DTOs;

public class PostDto
{
    public Guid UserId { get; set; }

    public string Content { get; set; }

    public PostType Type { get; set; }

    public Guid? GoalId { get; set; }

    public Guid? SkillId { get; set; }
}
