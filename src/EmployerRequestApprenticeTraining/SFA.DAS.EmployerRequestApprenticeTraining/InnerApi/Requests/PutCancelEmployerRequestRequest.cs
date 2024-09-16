using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests
{
    public class PutCancelEmployerRequestRequest : IPutApiRequest
    {
        public Guid EmployerRequestId { get; set; }

        public PutCancelEmployerRequestRequestData Data { get; set; }

        public PutCancelEmployerRequestRequest(Guid employerRequestId, PutCancelEmployerRequestRequestData data)
        {
            EmployerRequestId = employerRequestId;
            Data = data;
        }

        public string PutUrl => $"api/employer-requests/{EmployerRequestId}/cancel";

        object IPutApiRequest.Data { get => Data; set => Data = value as PutCancelEmployerRequestRequestData; }
    }

    public class PutCancelEmployerRequestRequestData
    {
        public Guid CancelledBy { get; set; }
    }
}
