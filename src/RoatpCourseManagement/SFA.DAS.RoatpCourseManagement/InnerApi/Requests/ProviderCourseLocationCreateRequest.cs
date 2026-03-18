using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.CreateProviderLocation;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class ProviderCourseLocationCreateRequest : IPostApiRequest
    {
        public readonly int Ukprn;
        public readonly string LarsCode;
        public string PostUrl => $"providers/{Ukprn}/courses/{LarsCode}/locations";
        public object Data { get; set; }
        public ProviderCourseLocationCreateRequest(AddProviderCourseLocationCommand data)
        {
            Ukprn = data.Ukprn;
            LarsCode = data.LarsCode;
            Data = data;
        }
    }
}
