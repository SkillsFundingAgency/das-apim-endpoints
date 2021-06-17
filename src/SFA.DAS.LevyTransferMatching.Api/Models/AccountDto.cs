using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class AccountDto
    {
        public double RemainingTransferAllowance { get; set; }

        public static implicit operator AccountDto(Account account)
        {
            return new AccountDto()
            {
                RemainingTransferAllowance = account.RemainingTransferAllowance,
            };
        }
    }
}