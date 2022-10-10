using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class AddNationalLocationToProviderCourseLocationsRequest : IPostApiRequest, IRequest<Unit>
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }
        public string PostUrl => $"/providers/{Ukprn}/courses/{LarsCode}/locations/national";

        public object Data { get; set; }

        public AddNationalLocationToProviderCourseLocationsRequest(int ukprn, int larsCode, string userId)
        {
            Ukprn = ukprn;
            LarsCode = larsCode;
            UserId = userId;
            Data = new AddNationalLocationToProviderCourseModel(userId);
        }
    }
}
