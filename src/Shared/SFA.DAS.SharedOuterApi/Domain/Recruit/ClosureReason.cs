using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Domain.Recruit;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ClosureReason
{
    Auto,
    Manual,
    TransferredByQa,
    BlockedByQa,
    TransferredByEmployer,
    WithdrawnByQa
}