using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.CreateProviderCourse;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Web;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class ProviderCourseCreateRequest : IPostApiRequest
    {
        public readonly int Ukprn;
        public readonly int LarsCode;
        private readonly string UserId;
        private readonly string UserDisplayName;
        public string PostUrl => $"providers/{Ukprn}/courses/{LarsCode}/?userId={HttpUtility.UrlEncode(UserId)}&userDisplayName={HttpUtility.UrlEncode(UserDisplayName)}";
        public object Data { get; set; }
        public ProviderCourseCreateRequest(CreateProviderCourseCommand data)
        {
            Ukprn = data.Ukprn;
            LarsCode = data.LarsCode;
            UserId = data.UserId;
            UserDisplayName = data.UserDisplayName;
            Data = data;
        }
    }
}
