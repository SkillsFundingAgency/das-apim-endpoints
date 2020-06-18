using SFA.DAS.FindApprenticeshipTraining.Application.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Domain.InnerApi.Requests
{
    public class GetStandardsListRequest : IGetApiRequest
    {
        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}api/courses/standards";
    }
}
