using SkillSphere.Posts.Core.Enums;

namespace SkillSphere.Posts.Core.Models;

public class Post
{
    public Guid Id { get; init; }

    public Guid UserId { get; private set; }

    public string Content { get; private set; }

    public PostType Type { get; private set; }

    public Guid? GoalId { get; set; }

    public Guid? SkillId { get; set; }

    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; private set; }

    public Post(Guid userId, string content, PostType type, Guid? goalId = null, Guid? skillId = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Content = content;
        Type = type;
        GoalId = goalId;
        SkillId = skillId;
        CreatedAt = DateTime.UtcNow;
    }

    public Post(Guid id, Guid userId, string content, PostType type, DateTime createdAt,
        Guid? goalId = null, Guid? skillId = null, DateTime? updatedAt = null)
    {
        Id = id;
        UserId = userId;
        Content = content;
        Type = type;
        GoalId = goalId;
        SkillId = skillId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public void UpdatePost(string content, PostType type)
    {
        Content = content;
        Type = type;
        UpdatedAt = DateTime.UtcNow;
    }
}
