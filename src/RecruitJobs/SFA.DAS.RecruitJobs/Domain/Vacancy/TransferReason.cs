using System.Text.Json.Serialization;

namespace SFA.DAS.RecruitJobs.Domain.Vacancy;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransferReason
{
    EmployerRevokedPermission,
    BlockedByQa
}