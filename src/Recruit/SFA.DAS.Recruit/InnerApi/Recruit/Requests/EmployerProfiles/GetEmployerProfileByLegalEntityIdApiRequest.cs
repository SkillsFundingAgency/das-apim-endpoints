using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;

public record GetEmployerProfileByLegalEntityIdApiRequest(long AccountLegalEntityId) : IGetApiRequest
{
    public string GetUrl => $"api/employer/profiles/{AccountLegalEntityId}";
}