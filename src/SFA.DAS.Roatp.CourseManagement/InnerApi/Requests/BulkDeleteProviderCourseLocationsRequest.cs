using SFA.DAS.Roatp.CourseManagement.InnerApi.Models.DeleteProviderCourseLocations;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Web;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    public class BulkDeleteProviderCourseLocationsRequest : IDeleteApiRequest
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }
        public DeleteProviderCourseLocationOption DeleteProviderCourseLocationOption { get; set; }

        public string DeleteUrl => $"/providers/{Ukprn}/courses/{LarsCode}/locations?options={DeleteProviderCourseLocationOption}&userId={HttpUtility.UrlEncode(UserId)}";
    }
}
