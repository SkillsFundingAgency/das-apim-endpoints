using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetVacancyReviewsCountByAccountLegalEntityRequest(long accountLegalEntityId, string? status, string? manualOutcome, string? employerNameOption) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{accountLegalEntityId}/vacancyreviews/count?status={status}&manualOutcome={manualOutcome}&employerNameOption={employerNameOption}";
}
