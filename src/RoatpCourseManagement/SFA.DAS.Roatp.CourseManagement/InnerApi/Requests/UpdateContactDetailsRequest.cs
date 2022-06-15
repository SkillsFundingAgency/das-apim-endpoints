using SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateContactDetails;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
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
