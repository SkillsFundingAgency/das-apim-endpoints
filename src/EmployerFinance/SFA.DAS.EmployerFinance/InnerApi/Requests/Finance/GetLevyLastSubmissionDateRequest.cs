using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;

public sealed record GetLevyLastSubmissionDateRequest(long AccountId) : IGetApiRequest
{
    public string GetUrl => $"api/levy-declarations/{AccountId}/last-submission-date";
}