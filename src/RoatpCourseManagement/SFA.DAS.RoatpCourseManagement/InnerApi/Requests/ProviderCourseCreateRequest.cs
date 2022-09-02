using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.CreateProviderCourse;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class ProviderCourseCreateRequest : IPostApiRequest
    {
        public readonly int Ukprn;
        public readonly int LarsCode;
        public string PostUrl => $"providers/{Ukprn}/courses/{LarsCode}/";
        public object Data { get; set; }
        public ProviderCourseCreateRequest(CreateProviderCourseCommand data)
        {
            Ukprn = data.Ukprn;
            LarsCode = data.LarsCode;
            Data = data;
        }
    }
}
