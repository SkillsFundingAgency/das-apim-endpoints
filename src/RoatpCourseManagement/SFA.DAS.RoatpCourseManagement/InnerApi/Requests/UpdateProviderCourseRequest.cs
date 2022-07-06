using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class UpdateProviderCourseRequest : IPutApiRequest<ProviderCourseUpdateModel>
    {
        public int Ukprn { get; }
        public int LarsCode { get; }
        public string UserId { get; set; }
        public string PutUrl => $"providers/{Ukprn}/courses/{LarsCode}/";

        public UpdateProviderCourseRequest(ProviderCourseUpdateModel data)
        {
            Ukprn = data.Ukprn;
            LarsCode = data.LarsCode;
            UserId = data.UserId;
            Data = data;
        }

        public ProviderCourseUpdateModel Data { get; set; }
    }
}
