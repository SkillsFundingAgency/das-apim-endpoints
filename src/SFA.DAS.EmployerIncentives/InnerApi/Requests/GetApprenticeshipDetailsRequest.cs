using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetApprenticeshipDetailsRequest : IGetApiRequest
    {
        private readonly long _apprenticeshipId;

        public GetApprenticeshipDetailsRequest(long apprenticeshipId)
        {
            _apprenticeshipId = apprenticeshipId;
        }

        public string BaseUrl { get; set; }

        public string GetUrl =>
            $"{BaseUrl}api/apprenticeships/{_apprenticeshipId}";
    }
}