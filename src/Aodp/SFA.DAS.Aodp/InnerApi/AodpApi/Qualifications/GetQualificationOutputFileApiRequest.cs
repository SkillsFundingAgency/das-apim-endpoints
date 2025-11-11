using SFA.DAS.Aodp.Application.Commands.Qualification;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications
{
    public class GetQualificationOutputFileApiRequest : IPostApiRequest
    {
        public string PostUrl => $"api/qualifications/outputfile";
        public object Data { get; set; }

        public GetQualificationOutputFileApiRequest(GetQualificationOutputFileQuery data)
        {
            Data = data;
        }
    }
}
