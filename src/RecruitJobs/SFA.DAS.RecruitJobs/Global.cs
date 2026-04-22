using System.Text.Json;

namespace SFA.DAS.RecruitJobs;

public static class Global
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
}