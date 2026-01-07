using System.Text.Json;

namespace SFA.DAS.Recruit;

public static class Global
{
    public static JsonSerializerOptions JsonSerializerOptionsCaseInsensitive => new() { PropertyNameCaseInsensitive = true };
}