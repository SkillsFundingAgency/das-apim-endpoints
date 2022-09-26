using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PostReinstatePaymentsRequest : IPostApiRequest
    {
        public PostReinstatePaymentsRequest(ReinstatePaymentsRequest request)
        {
            Data = request;
        }

        public string PostUrl => $"reinstate-payments";
        public object Data { get; set; }
    }

    public class ReinstatePaymentsRequest
    {
        public List<Guid> Payments { get; set; }
        public ReinstatePaymentsServiceRequest ServiceRequest { get; set; }
    }

    public class ReinstatePaymentsServiceRequest
    {
        public string TaskId { get; set; }
        public string DecisionReference { get; set; }
        public DateTime? TaskCreatedDate { get; set; }
        public string Process { get; set; }
    }
}