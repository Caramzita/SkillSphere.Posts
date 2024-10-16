using SkillSphere.Posts.Contracts.DTOs;
using System.Net.Http.Json;

namespace SkillSphere.Posts.UseCases.Services;

public class UserProfileServiceClient
{
    private readonly HttpClient _httpClient;

    public UserProfileServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GoalDto?> GetGoalByIdAsync(Guid goalId)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7295/api/users/profile/goals/{goalId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<GoalDto>();
        }

        return null;
    }

    public async Task<List<SkillDto>?> GetSkillsByIdsAsync(List<Guid> skillIds)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7295/api/skills/check-skills", skillIds);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<SkillDto>>();
        }

        return null;
    }
}
