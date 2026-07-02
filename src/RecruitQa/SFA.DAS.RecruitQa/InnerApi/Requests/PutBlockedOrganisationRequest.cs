using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class PutBlockedOrganisationRequest(Guid id, BlockedOrganisationDto data) : IPutApiRequest
{
    public string PutUrl => $"/api/blockedorganisations/{id}";
    public object Data { get; set; } = data;
}