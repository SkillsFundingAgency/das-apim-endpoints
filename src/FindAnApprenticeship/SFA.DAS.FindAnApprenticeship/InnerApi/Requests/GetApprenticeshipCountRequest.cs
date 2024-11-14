using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests
{
    public class GetApprenticeshipCountRequest(WageType? wageType = null) : IGetApiRequest
    {
        public string GetUrl => $"/api/vacancies/count?wageType={wageType}";
            
        public string Version => "2.0";
    }
}