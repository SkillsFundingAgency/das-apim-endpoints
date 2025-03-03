using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests
{
    public class PutAcknowledgeProviderResponsesRequest : IPutApiRequest<PutAcknowledgeProviderResponsesRequestData>
    {
        public Guid EmployerRequestId { get; set; }

        public PutAcknowledgeProviderResponsesRequestData Data { get; set; }

        public PutAcknowledgeProviderResponsesRequest(Guid employerRequestId, PutAcknowledgeProviderResponsesRequestData data)
        {
            EmployerRequestId = employerRequestId;
            Data = data;
        }

        public string PutUrl => $"api/employer-requests/{EmployerRequestId}/responses/acknowledge";
    }

    public class PutAcknowledgeProviderResponsesRequestData
    {
        public Guid AcknowledgedBy { get; set; }
    }
}
