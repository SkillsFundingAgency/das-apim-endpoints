using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourse;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Web;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class DeleteProviderCourseRequest : IDeleteApiRequest
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }

       
        public string DeleteUrl => $"/providers/{Ukprn}/courses/{LarsCode}/?userId={HttpUtility.UrlEncode(UserId)}";
    }
}
