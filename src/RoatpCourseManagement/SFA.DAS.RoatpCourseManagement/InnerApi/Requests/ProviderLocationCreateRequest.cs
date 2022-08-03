using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.CreateProviderLocation;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class ProviderLocationCreateRequest : IPostApiRequest
    {
        public readonly int Ukprn;
        public string PostUrl => $"providers/{Ukprn}/locations";
        public object Data { get; set; }
        public ProviderLocationCreateRequest(CreateProviderLocationCommand data)
        {
            Ukprn = data.Ukprn;
            Data = data;
        }
    }
}
