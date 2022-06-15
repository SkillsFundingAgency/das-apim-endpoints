using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateContactDetails;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class UpdateContactDetailsRequest : IPostApiRequest
    {
        public int Ukprn { get; }
        public int LarsCode { get; }
        public string PostUrl => $"providers/{Ukprn}/courses/{LarsCode}/";

        public UpdateContactDetailsRequest(UpdateContactDetailsCommand data)
        {
            Ukprn = data.Ukprn;
            LarsCode = data.LarsCode;
            Data = data;
        }

        public object Data { get; set; }
    }
}
