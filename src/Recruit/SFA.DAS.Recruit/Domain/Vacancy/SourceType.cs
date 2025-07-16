using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.Domain.Vacancy;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SourceType
{
    Clone,
    Extension,
    New
}