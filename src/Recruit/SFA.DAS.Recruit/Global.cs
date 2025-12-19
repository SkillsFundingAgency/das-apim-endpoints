using System.Text.Json;

namespace SFA.DAS.Recruit;

public class Global
{
    public static JsonSerializerOptions JsonSerializerOptions => new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
}