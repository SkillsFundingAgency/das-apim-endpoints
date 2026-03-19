using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public sealed record GetBlockedOrganisationByOrganisationIdRequest(string OrganisationId) : IGetApiRequest
{
    public string GetUrl => $"api/blockedorganisations/byorganisationid/{OrganisationId}";
}