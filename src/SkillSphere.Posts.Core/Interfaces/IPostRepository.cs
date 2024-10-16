using SkillSphere.Posts.Core.Enums;
using SkillSphere.Posts.Core.Models;

namespace SkillSphere.Posts.Core.Interfaces;

public interface IPostRepository
{
    IAsyncEnumerable<Post> GetAllPosts(bool orderByDescending = true);

    IAsyncEnumerable<Post> GetAllUserPosts(Guid userId, bool orderByDescending = true);

    IAsyncEnumerable<Post> GetPostsByGoalId(Guid goalId, bool orderByDescending = true);

    IAsyncEnumerable<Post> GetPostsByPostType(PostType postType, bool orderByDescending = true);

    IAsyncEnumerable<Post> GetPostsBySkillIds(List<Guid> skillIds, bool orderByDescending = true);

    Task<Post?> GetPostById(Guid postId);

    Task CreatePost(Post post);

    Task DeletePost(Post post);

    Task UpdatePost(Post post);
}
