using Microsoft.EntityFrameworkCore;
using SkillSphere.Posts.Core.Enums;
using SkillSphere.Posts.Core.Interfaces;
using SkillSphere.Posts.Core.Models;

namespace SkillSphere.Posts.DataAccess.Repositories;

public class PostRepository : IPostRepository
{
    private readonly DatabaseContext _context;

    public PostRepository(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    private IQueryable<Post> GetBaseQuery()
    {
        return _context.Posts.AsNoTracking(); 
    }

    public IAsyncEnumerable<Post> GetAllPosts()
    {
        return GetBaseQuery().AsAsyncEnumerable();
    }

    public IAsyncEnumerable<Post> GetAllUserPosts(Guid userId)
    {
        return GetBaseQuery()
            .Where(post => post.UserId == userId)
            .AsAsyncEnumerable();
    }

    public async Task<Post?> GetPostById(Guid postId)
    {
        return await _context.Posts
            .FirstOrDefaultAsync(post => post.Id == postId);
    }

    public IAsyncEnumerable<Post> GetPostsByGoalId(Guid goalId)
    {
        return GetBaseQuery()
            .Where(post => post.GoalId == goalId)
            .AsAsyncEnumerable();
    }

    public IAsyncEnumerable<Post> GetPostsByPostType(PostType postType)
    {
        return GetBaseQuery()
            .Where(post => post.Type == postType)
            .AsAsyncEnumerable();
    }

    public IAsyncEnumerable<Post> GetPostsBySkillId(Guid skillId)
    {
        return GetBaseQuery()
            .Where(post => post.SkillId == skillId)
            .AsAsyncEnumerable();
    }

    public async Task CreatePost(Post post)
    {
        await _context.AddAsync(post).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeletePost(Post post)
    {
        _context.Remove(post);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdatePost(Post post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}