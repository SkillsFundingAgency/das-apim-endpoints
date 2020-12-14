using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PostWithdrawApplicationRequest : IPostApiRequest<WithdrawRequest>
    {
        public PostWithdrawApplicationRequest(WithdrawRequest request)
        {
            Data = request;
        }

        public string PostUrl => $"withdrawls";
        public WithdrawRequest Data { get; set; }
    }

    public enum WithdrawlType
    {
        Employer = 1
    }

    public class WithdrawRequest
    {
        public WithdrawlType WithdrawlType { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long ULN { get; set; }
        public ServiceRequest ServiceRequest { get; set; }       
    }

    public class ServiceRequest
    {
        public string TaskId { get; set; }
        public string DecisionReference { get; set; }
        public DateTime? TaskCreatedDate { get; set; }
    }    
}
