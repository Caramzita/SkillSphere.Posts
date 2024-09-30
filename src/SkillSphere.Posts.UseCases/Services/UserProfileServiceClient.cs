using SkillSphere.Posts.Contracts.DTOs;
using System.Net.Http.Json;

namespace SkillSphere.Posts.UseCases.Services;

public class UserProfileServiceClient
{
    private readonly HttpClient _httpClient;

    private readonly string _userProfileService = "https://localhost:7295/api/profiles";

    public UserProfileServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GoalDto?> GetGoalByIdAsync(Guid goalId)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7295/api/profiles/goals/{goalId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<GoalDto>();
        }

        return null;
    }

    public async Task<SkillDto?> GetSkillByIdAsync(Guid skillId)
    {
        var response = await _httpClient.GetAsync($"{_userProfileService}/skills/{skillId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<SkillDto>();
        }

        return null;
    }
}
