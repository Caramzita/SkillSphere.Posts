using FluentValidation;

namespace SkillSphere.Posts.UseCases.Posts.Commands;

public class BasePostCommandValidator<T> : AbstractValidator<T>
    where T : IPostCommand
{
    public BasePostCommandValidator()
    {
        RuleFor(command => command.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MinimumLength(5).WithMessage("Content must be at least 5 characters.")
            .MaximumLength(500).WithMessage("Content must not exceed 500 characters.");

        RuleFor(command => command.Type)
            .IsInEnum().WithMessage("Type must be a valid PostType.");

        RuleFor(command => command.SkillIds)
            .Must(skillIds => skillIds == null || skillIds.All(id => id != Guid.Empty))
            .WithMessage("Skill IDs must not contain empty GUIDs.");
    }
}
