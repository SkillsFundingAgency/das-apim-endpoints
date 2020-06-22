using SFA.DAS.FindApprenticeshipTraining.Application.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Requests
{
    public class GetStandardsListRequest : IGetApiRequest
    {
        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}api/courses/standards";
    }
}
