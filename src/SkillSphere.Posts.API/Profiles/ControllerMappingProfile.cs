using AutoMapper;
using SkillSphere.Posts.Contracts.DTOs;
using SkillSphere.Posts.UseCases.Posts.Commands.AddPost;
using SkillSphere.Posts.UseCases.Posts.Commands.UpdatePost;

namespace SkillSphere.Posts.API.Profiles;

/// <summary>
/// Профиль AutoMapper для маппинга объектов запросов из контроллера на команды.
/// </summary>
public class ControllerMappingProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ControllerMappingProfile"/> и задает конфигурации маппинга.
    /// </summary>
    public ControllerMappingProfile()
    {
        CreateMap<PostRequestDto, AddPostCommand>();

        CreateMap<PostRequestDto, UpdatePostCommand>();
    }
}
