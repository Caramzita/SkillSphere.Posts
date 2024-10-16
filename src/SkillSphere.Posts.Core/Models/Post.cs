using SkillSphere.Posts.Core.Enums;

namespace SkillSphere.Posts.Core.Models;

public class Post
{
    public Guid Id { get; init; }

    public Guid UserId { get; private set; }

    public string Content { get; private set; }

    public PostType Type { get; private set; }

    public Guid? GoalId { get; set; }

    public List<Guid>? SkillIds { get; set; }

    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; private set; }

    public Post(Guid userId, string content, PostType type, Guid? goalId = null, List<Guid>? skillIds = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Content = content;
        Type = type;
        GoalId = goalId;
        SkillIds = skillIds;
        CreatedAt = DateTime.UtcNow;
    }

    public Post(Guid id, Guid userId, string content, PostType type, DateTime createdAt,
        Guid goalId, List<Guid> skillIds, DateTime updatedAt)
    {
        Id = id;
        UserId = userId;
        Content = content;
        Type = type;
        GoalId = goalId;
        SkillIds = skillIds;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public void UpdatePost(string content, PostType type, List<Guid>? skillIds = null)
    {
        Content = content;
        Type = type;
        UpdatedAt = DateTime.UtcNow;
        SkillIds = skillIds;
    }
}
