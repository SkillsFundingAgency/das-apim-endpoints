using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunities.GetConfirmation;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Opportunities
{
    public class GetConfirmationResponse
    {
        public string AccountName { get; set; }
        public bool IsNamePublic { get; set; }

        public static implicit operator GetConfirmationResponse(GetConfirmationQueryResult result)
        {
            return new GetConfirmationResponse
            {
                AccountName = result.AccountName,
                IsNamePublic = result.IsNamePublic
            };
        }
    }
}
