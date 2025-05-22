using System.Text.Json.Serialization;

namespace SFA.DAS.FindApprenticeshipJobs.Domain.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApprenticeshipTypes
{
    Standard,
    Foundation,
}