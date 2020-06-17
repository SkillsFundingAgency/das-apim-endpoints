using SFA.DAS.FindApprenticeshipTraining.Application.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Domain.InnerApi.Requests
{
    public class GetStandardsListRequest : IGetAllApiRequest
    {
        public string BaseUrl { get; set; }
        public string GetAllUrl => $"{BaseUrl}api/courses/standards";
    }
}
