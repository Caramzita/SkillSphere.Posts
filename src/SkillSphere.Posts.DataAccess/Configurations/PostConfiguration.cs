using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SkillSphere.Posts.Core.Models;

namespace SkillSphere.Posts.DataAccess.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Content)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.Property(p => p.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.GoalId)
            .IsRequired(false);

        builder.Property(p => p.SkillId)
            .IsRequired(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired(false);
    }
}
