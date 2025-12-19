using System.Text.Json;

namespace SFA.DAS.Recruit;

public static class Global
{
    public static JsonSerializerOptions JsonSerializerOptionsCamelCase => new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    public static JsonSerializerOptions JsonSerializerOptionsCaseInsensitive => new() { PropertyNameCaseInsensitive = true };
}