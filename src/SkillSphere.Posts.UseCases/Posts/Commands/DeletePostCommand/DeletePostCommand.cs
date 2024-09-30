using MediatR;
using SkillSphere.Infrastructure.UseCases;

namespace SkillSphere.Posts.UseCases.Posts.Commands.DeletePostCommand;

public class DeletePostCommand : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DeletePostCommand(Guid id, Guid userId)
    {
        Id = id;
        UserId= userId;
    }
}
