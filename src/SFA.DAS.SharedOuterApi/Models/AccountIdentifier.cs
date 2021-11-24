using System;

namespace SFA.DAS.SharedOuterApi.Models
{
    public class AccountIdentifier
    {
        public AccountIdentifier(string accountIdentifier)
        {
            AccountPublicHashedId = null;
            Ukprn = null;
            
            var identifierParts = accountIdentifier?.Split('-');
            
            if (string.IsNullOrEmpty(accountIdentifier) || identifierParts.Length != 3)
            {
                AccountType = AccountType.Unknown;
                return;
            }
            var id = identifierParts[1];
            
            Enum.TryParse(typeof(AccountType), identifierParts[0], true,
                out var accountType);

            AccountType = (AccountType?) accountType ?? AccountType.Unknown;
            switch (AccountType)
            {
                case AccountType.Provider when int.TryParse(id, out var providerId):
                    Ukprn = providerId;
                    break;
                case AccountType.Employer:
                    AccountPublicHashedId = id.ToUpper();
                    break;
                case AccountType.Unknown:
                    break;
            }
        }

        public AccountType AccountType { get; }
        public string AccountPublicHashedId { get; }
        public int? Ukprn { get; }
    }

    public enum AccountType
    {
        Employer = 0,
        Provider = 1,
        Unknown = 3
    }
}