using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerFeedback;

public class GetEmployerFeedbackDetailsAnnualRequest(long ukprn) : IGetApiRequest
{
    public string GetUrl => $"api/EmployerFeedbackResult/{_ukprn}/annual";
    private long _ukprn { get; } = ukprn;
}
