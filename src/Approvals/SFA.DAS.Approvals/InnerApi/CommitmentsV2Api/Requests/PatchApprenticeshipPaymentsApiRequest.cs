using System;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;

public class PatchApprenticeshipPaymentsApiRequest(long apprenticeshipId, PatchApprenticeshipPaymentsApiRequest.Body requestBody)
    : IPatchApiRequest<PatchApprenticeshipPaymentsApiRequest.Body>
{
    public string PatchUrl => $"api/apprenticeships/{apprenticeshipId}/payments";

    public Body Data { get; set; } = requestBody;

    public class Body
    {
        public UserInfo UserInfo { get; set; }

        public DateTime? PaymentFreezeDate { get; set; }

        public int? FreezePaymentsReason { get; set; }

        public int Party { get; set; }
    }
}
