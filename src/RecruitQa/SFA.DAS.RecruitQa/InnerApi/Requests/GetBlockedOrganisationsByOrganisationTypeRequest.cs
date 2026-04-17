using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetBlockedOrganisationsByOrganisationTypeRequest(string organisationType) : IGetAllApiRequest
{
    public string GetAllUrl => $"api/blockedorganisations?organisationType={organisationType}";
}