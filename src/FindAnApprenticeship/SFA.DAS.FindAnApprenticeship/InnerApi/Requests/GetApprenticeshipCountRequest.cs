using System.Text;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests
{
    public class GetApprenticeshipCountRequest : IGetApiRequest
    {
        private readonly StringBuilder _queryBuilder = new();

        public GetApprenticeshipCountRequest(WageType? wageType = null)
        {
            _queryBuilder.Append("/api/vacancies/count");
            if (wageType is not null) _queryBuilder.Append($"?wageType={wageType}");
        }

        public string GetUrl => _queryBuilder.ToString();
            
        public string Version => "2.0";
    }
}