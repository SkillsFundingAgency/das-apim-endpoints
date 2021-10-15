using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class AccountDto
    {
        public string EncodedAccountId { get; set; }
        public string DasAccountName { get; set; }
        public decimal? RemainingTransferAllowance { get; set; }

        public static implicit operator AccountDto(Account account)
        {
            return new AccountDto
            {
                EncodedAccountId = account.EncodedAccountId,
                DasAccountName = account.DasAccountName,
                RemainingTransferAllowance = account.RemainingTransferAllowance
            };
        }
    }
}