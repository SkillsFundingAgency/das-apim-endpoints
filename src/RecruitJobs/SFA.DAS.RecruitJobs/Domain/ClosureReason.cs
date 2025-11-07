using System.Text.Json.Serialization;

namespace SFA.DAS.RecruitJobs.Domain;

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