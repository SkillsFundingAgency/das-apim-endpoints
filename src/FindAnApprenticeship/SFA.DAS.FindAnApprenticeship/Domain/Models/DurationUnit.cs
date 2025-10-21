using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.Domain.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DurationUnit
{
    Week,
    Month,
    Year
}