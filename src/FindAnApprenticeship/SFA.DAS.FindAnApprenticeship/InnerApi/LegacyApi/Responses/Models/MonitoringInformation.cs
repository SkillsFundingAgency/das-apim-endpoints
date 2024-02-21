using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Enums;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Models;

public record MonitoringInformation
{
    public Gender? Gender { get; set; }

    public DisabilityStatus? DisabilityStatus { get; set; }

    public int? Ethnicity { get; set; }
}