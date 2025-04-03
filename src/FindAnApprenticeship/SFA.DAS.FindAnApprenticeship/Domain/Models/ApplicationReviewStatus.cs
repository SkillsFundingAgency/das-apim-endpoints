using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.Domain.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApplicationReviewStatus
{
    New,
}