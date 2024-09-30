using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillSphere.Posts.Core.Models;
using System.Reflection;

namespace SkillSphere.Posts.DataAccess;

public class DatabaseContext : DbContext
{
    private readonly ILogger<DatabaseContext> _logger;

    public DbSet<Post> Posts { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options, ILogger<DatabaseContext> logger)
        : base(options)
    {
        _logger = logger;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            _logger.LogWarning("DbContextOptionsBuilder is not configured.");
        }

        optionsBuilder.EnableSensitiveDataLogging()
                      .LogTo(log => _logger.LogInformation(log));
    }
}
