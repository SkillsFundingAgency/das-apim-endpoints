using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests
{
    public class GetTotalPositionsAvailableRequest : IGetApiRequest
    {
        public string GetUrl => "api/vacancies/total-positions-available";
    }
}