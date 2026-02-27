using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Domain.Recruit;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SourceType
{
    Clone,
    Extension,
    New
}