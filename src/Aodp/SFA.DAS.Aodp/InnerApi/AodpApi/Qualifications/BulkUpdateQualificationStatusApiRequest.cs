using SFA.DAS.AODP.Application.Commands.Qualifications;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Apim.Shared.Interfaces;

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