using SFA.DAS.AODP.Application.Commands.Qualifications;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AODP.Domain.Qualifications.Requests
{
    [ExcludeFromCodeCoverage]
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