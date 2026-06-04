using System.Text.Json;

namespace SFA.DAS.RecruitQa;

public static class Global
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true };
}