using System;
using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Domain.Recruit;

public class TransferInfo
{
    public long Ukprn { get; set; }
    public string ProviderName { get; set; }
    public string LegalEntityName { get; set; }
    public VacancyUser TransferredByUser { get; set; }
    public DateTime TransferredDate { get; set; }
    public TransferReason Reason { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransferReason
{
    EmployerRevokedPermission,
    BlockedByQa
}