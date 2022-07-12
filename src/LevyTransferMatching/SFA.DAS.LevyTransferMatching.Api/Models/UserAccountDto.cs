using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class UserAccountDto
    {
        public string EncodedAccountId { get; set; }
        public string DasAccountName { get; set; }

        public static implicit operator UserAccountDto(UserAccount userAccount)
        {
            return new UserAccountDto
            {
                EncodedAccountId = userAccount.EncodedAccountId,
                DasAccountName = userAccount.DasAccountName,
            };
        }
    }
}