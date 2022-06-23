using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class UpdateProviderCourseRequest : IPostApiRequest
    {
        public int Ukprn { get; }
        public int LarsCode { get; }
        public string UserId { get; set; }
        public string PostUrl => $"providers/{Ukprn}/courses/{LarsCode}/";

        public UpdateProviderCourseRequest(ProviderCourseUpdateModel data)
        {
            Ukprn = data.Ukprn;
            LarsCode = data.LarsCode;
            UserId = data.UserId;
            Data = data;
        }

        public object Data { get; set; }
    }
}
