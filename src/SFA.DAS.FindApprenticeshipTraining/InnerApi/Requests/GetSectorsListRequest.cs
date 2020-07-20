using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetSectorsListRequest : IGetApiRequest
    {
        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}api/courses/sectors";
    }
}