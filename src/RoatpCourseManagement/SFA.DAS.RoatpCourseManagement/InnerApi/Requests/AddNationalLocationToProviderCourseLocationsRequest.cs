using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class AddNationalLocationToProviderCourseLocationsRequest : IPostApiRequest
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string PostUrl => $"/providers/{Ukprn}/courses/{LarsCode}/locations/national";

        public object Data { get; set; }

        public AddNationalLocationToProviderCourseLocationsRequest(int ukprn, int larsCode, string userId, string userDisplayName)
        {
            Ukprn = ukprn;
            LarsCode = larsCode;
            UserId = userId;
            UserDisplayName = userDisplayName;
            Data = new AddNationalLocationToProviderCourseModel(userId, userDisplayName);
        }
    }
}
