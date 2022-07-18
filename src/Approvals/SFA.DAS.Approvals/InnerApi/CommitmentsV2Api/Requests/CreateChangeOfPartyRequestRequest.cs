using System;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Types;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests
{
    public class CreateChangeOfPartyRequestRequest: IPostApiRequest
    {
        public long ApprenticeshipId { get; }

        public CreateChangeOfPartyRequestRequest(long apprenticeshipId, Body body)
        {
            ApprenticeshipId = apprenticeshipId;
            Data = body;
        }
        
        public string PostUrl => $"api/apprenticeships/{ApprenticeshipId}/change-of-party-requests";
        public object Data { get; set; }

        public class Body
        {
            public ChangeOfPartyRequestType ChangeOfPartyRequestType { get; set; }
            public long NewPartyId { get; set; }
            public int? NewPrice { get; set; }
            public DateTime? NewStartDate { get; set; }
            public DateTime? NewEndDate { get; set; }
            public DateTime? NewEmploymentEndDate { get; set; }
            public int? NewEmploymentPrice { get; set; }
            public DeliveryModel? DeliveryModel { get; set; }
            public UserInfo UserInfo { get; set; }
        }
    }
}
