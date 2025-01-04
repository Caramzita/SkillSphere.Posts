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

    public IAsyncEnumerable<Post> GetAllPosts(bool orderByDescending = true)
    {
        var query = _context.Posts.AsNoTracking();
        return ApplySorting(query, orderByDescending).AsAsyncEnumerable();
    }

    public IAsyncEnumerable<Post> GetAllUserPosts(Guid userId, bool orderByDescending = true)
    {
        var query = _context.Posts.AsNoTracking()
            .Where(post => post.UserId == userId);

        return ApplySorting(query, orderByDescending).AsAsyncEnumerable();
    }

    public async Task<Post?> GetPostById(Guid postId)
    {
        return await _context.Posts.AsNoTracking()
            .FirstOrDefaultAsync(post => post.Id == postId)
            .ConfigureAwait(false);
    }

    public IAsyncEnumerable<Post> GetPostsByGoalId(Guid goalId, bool orderByDescending = true)
    {
        var query = _context.Posts.AsNoTracking()
            .Where(post => post.GoalId == goalId);

        return ApplySorting(query, orderByDescending).AsAsyncEnumerable();
    }

    public IAsyncEnumerable<Post> GetPostsByPostType(PostType postType, bool orderByDescending = true)
    {
        var query = _context.Posts.AsNoTracking()
            .Where(post => post.Type == postType);

        return ApplySorting(query, orderByDescending).AsAsyncEnumerable();
    }

    public IAsyncEnumerable<Post> GetPostsBySkillIds(List<Guid> skillIds, bool orderByDescending = true)
    {
        var query = _context.Posts.AsNoTracking()
        .Where(post => skillIds.All(skillId => post.SkillIds!.Contains(skillId)) && post.SkillIds!.Count > 0);

        return ApplySorting(query, orderByDescending).AsAsyncEnumerable();
    }

    private static IQueryable<Post> ApplySorting(IQueryable<Post> query, bool orderByDescending)
    {
        return orderByDescending 
            ? query.OrderByDescending(post => post.CreatedAt) 
            : query.OrderBy(post => post.CreatedAt);
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