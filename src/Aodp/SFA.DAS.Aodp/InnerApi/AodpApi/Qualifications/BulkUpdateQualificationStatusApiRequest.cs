using SFA.DAS.AODP.Application.Commands.Qualifications;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.Qualifications.Requests
{
    public class BulkUpdateQualificationStatusApiRequest : IPutApiRequest
    {

        public BulkUpdateQualificationStatusApiRequest(BulkUpdateQualificationStatusCommand data)
        {
            Data = data;
        }

        public string PutUrl => $"api/qualifications/bulk-status";

        public object Data { get; set; }
    }
}