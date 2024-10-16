using AutoMapper;
using SkillSphere.Posts.Contracts.DTOs;
using SkillSphere.Posts.UseCases.Posts.Commands.AddPostCommand;
using SkillSphere.Posts.UseCases.Posts.Commands.UpdatePostCommand;

namespace SkillSphere.Posts.API;

public class ControllerMappingProfile : Profile
{
    public ControllerMappingProfile()
    {
        CreateMap<PostRequestDto, AddPostCommand>();

        CreateMap<PostRequestDto, UpdatePostCommand>();
    }
}
