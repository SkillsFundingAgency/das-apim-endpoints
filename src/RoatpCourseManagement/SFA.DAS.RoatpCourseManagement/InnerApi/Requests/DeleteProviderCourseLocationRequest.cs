using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class DeleteProviderCourseLocationRequest : IDeleteApiRequest
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }

        public string DeleteUrl => $"/providers/{Ukprn}/courses/{LarsCode}/location/{Id}";
    }
}
