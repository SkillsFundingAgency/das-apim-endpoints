using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PostRevertPaymentsRequest : IPostApiRequest
    {
        public PostRevertPaymentsRequest(RevertPaymentsRequest request)
        {
            Data = request;
        }

        public string PostUrl => $"revert-payments";
        public object Data { get; set; }
    }

    public class RevertPaymentsRequest
    {
        public List<Guid> Payments { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
    }
}
