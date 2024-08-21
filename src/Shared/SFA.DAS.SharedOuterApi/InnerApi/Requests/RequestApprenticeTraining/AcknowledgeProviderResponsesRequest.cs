using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining
{
    [ExcludeFromCodeCoverage]
    public class AcknowledgeProviderResponsesRequest : IPutApiRequest<AcknowledgeProviderResponsesRequestData>
    {
        public Guid EmployerRequestId { get; set; }

        public AcknowledgeProviderResponsesRequestData Data { get; set; }

        public AcknowledgeProviderResponsesRequest(Guid employerRequestId, AcknowledgeProviderResponsesRequestData data)
        {
            EmployerRequestId = employerRequestId;
            Data = data;
        }

        public string PutUrl => $"api/employerrequest/{EmployerRequestId}/acknowledge-responses";
    }

    [ExcludeFromCodeCoverage]
    public class AcknowledgeProviderResponsesRequestData
    {
        public Guid AcknowledgedBy { get; set; }
    }
}
