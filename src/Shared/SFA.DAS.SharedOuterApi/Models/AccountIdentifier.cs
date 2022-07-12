using System;
using System.Linq;

namespace SFA.DAS.SharedOuterApi.Models
{
    public class AccountIdentifier
    {
        public AccountIdentifier(string accountIdentifier)
        {
            AccountHashedId = null;
            Ukprn = null;
            
            var identifierParts = accountIdentifier?.Split('-');
            
            if (string.IsNullOrEmpty(accountIdentifier) || identifierParts.Length < 3)
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
                    AccountHashedId = id.ToUpper();
                    break;
                case AccountType.External:
                    if(Guid.TryParse(string.Join("-", identifierParts.Skip(1).Take(identifierParts.Length-2)), out var externalId))
                    {
                        ExternalId = externalId;    
                    }
                    else
                    {
                        AccountType = AccountType.Unknown;
                    }
                    break;
                case AccountType.Unknown:
                    break;
            }
        }

        public AccountType AccountType { get; }
        public string AccountHashedId { get; }
        public int? Ukprn { get; }
        public Guid ExternalId {get;}
    }

    public enum AccountType
    {
        Employer = 0,
        Provider = 1,
        External = 2,
        Unknown = 3
    }
}