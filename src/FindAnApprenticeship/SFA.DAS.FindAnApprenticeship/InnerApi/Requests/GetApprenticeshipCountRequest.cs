using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests
{
    public class GetApprenticeshipCountRequest : IGetApiRequest
    {
        public string GetUrl => "/api/vacancies/count";
    }
}