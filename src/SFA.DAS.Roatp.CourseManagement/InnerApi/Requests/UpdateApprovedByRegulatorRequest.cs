using SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateApprovedByRegulator;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    public class UpdateApprovedByRegulatorRequest : IPostApiRequest
    {
        public int Ukprn { get; }
        public int LarsCode { get; }
        public string PostUrl => $"providers/{Ukprn}/courses/{LarsCode}/update-approved-by-regulator";

        public UpdateApprovedByRegulatorRequest(UpdateApprovedByRegulatorCommand data)
        {
            Ukprn = data.Ukprn;
            LarsCode = data.LarsCode;
            Data = data;
        }

        public object Data { get; set; }
    }
}
