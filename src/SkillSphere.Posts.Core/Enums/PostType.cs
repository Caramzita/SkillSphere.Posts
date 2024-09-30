using System.Text.Json.Serialization;

namespace SkillSphere.Posts.Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PostType
{
    TextPost,
    GoalUpdate,
    EducationalMaterial,
    Task
}
