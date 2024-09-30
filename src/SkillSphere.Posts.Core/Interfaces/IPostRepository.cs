using SkillSphere.Posts.Core.Enums;
using SkillSphere.Posts.Core.Models;

namespace SkillSphere.Posts.Core.Interfaces;

public interface IPostRepository
{
    IAsyncEnumerable<Post> GetAllPosts();

    IAsyncEnumerable<Post> GetAllUserPosts(Guid userId);

    IAsyncEnumerable<Post> GetPostsByGoalId(Guid goalId);

    IAsyncEnumerable<Post>  GetPostsByPostType(PostType postType);

    IAsyncEnumerable<Post>  GetPostsBySkillId(Guid skillId);

    Task<Post?> GetPostById(Guid postId);

    Task CreatePost(Post post);

    Task DeletePost(Post post);

    Task UpdatePost(Post post);
}
