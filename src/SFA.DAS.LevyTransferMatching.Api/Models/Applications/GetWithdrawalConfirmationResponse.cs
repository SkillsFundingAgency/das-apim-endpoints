using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetWithdrawalConfirmation;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class GetWithdrawalConfirmationResponse
    {
        public string PledgeEmployerName { get; set; }
        public int PledgeId { get; set; }

        public static implicit operator GetWithdrawalConfirmationResponse(GetWithdrawalConfirmationQueryResult result)
        {
            return new GetWithdrawalConfirmationResponse()
            {
                PledgeEmployerName = result.PledgeEmployerName,
                PledgeId = result.PledgeId
            };
        }

    }
}
