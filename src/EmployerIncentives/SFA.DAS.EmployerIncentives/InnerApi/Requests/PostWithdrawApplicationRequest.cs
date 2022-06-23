using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PostWithdrawApplicationRequest : IPostApiRequest<WithdrawRequest>
    {
        public PostWithdrawApplicationRequest(WithdrawRequest request)
        {
            Data = request;
        }

        public string PostUrl => $"withdrawals";
        public WithdrawRequest Data { get; set; }
    }

    public enum WithdrawalType
    {
        Employer = 1,
        Compliance = 2
    }

    public class WithdrawRequest
    {
        public WithdrawalType WithdrawalType { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long ULN { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
        public long AccountId { get; set; }
        public string EmailAddress { get; set; }
    }
}
