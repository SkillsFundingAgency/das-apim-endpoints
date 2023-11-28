using System.Collections.Generic;
using System;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Types;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses
{
    public class GetChangeOfPartyRequestsResponse
    {
        public IReadOnlyCollection<ChangeOfPartyRequest> ChangeOfPartyRequests { get; set; }

        public class ChangeOfPartyRequest
        {
            public long Id { get; set; }

            public ChangeOfPartyRequestType ChangeOfPartyType { get; set; }

            public short OriginatingParty { get; set; }

            public byte Status { get; set; }

            public string EmployerName { get; set; }

            public DateTime? StartDate { get; set; }

            public DateTime? EndDate { get; set; }

            public int? Price { get; set; }

            public long? CohortId { get; set; }

            public short? WithParty { get; set; }

            public long? NewApprenticeshipId { get; set; }

            public long? ProviderId { get; set; }
        }
    }
}
