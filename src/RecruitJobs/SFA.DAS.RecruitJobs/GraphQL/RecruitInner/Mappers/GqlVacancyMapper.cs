using System.Text.Json;

namespace SFA.DAS.RecruitJobs.GraphQL.RecruitInner.Mappers;

// Note this class cannot be moved to the Shared assemblies as it
// relies upon the dynamically generated interface IAllVacancyFields. 
public static class GqlVacancyMapper
{
    private static T DeserializeOrNull<T>(string value) where T : class
    {
        return string.IsNullOrWhiteSpace(value) ? null : JsonSerializer.Deserialize<T>(value, Global.JsonSerializerOptions);
    }
    
    private static T? NullOrEnum<T>(string value) where T: struct
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return Enum.TryParse<T>(value, true, out var result) 
            ? result 
            : throw new InvalidOperationException($"Could not convert {value} to type {typeof(T)}");
    }    
}