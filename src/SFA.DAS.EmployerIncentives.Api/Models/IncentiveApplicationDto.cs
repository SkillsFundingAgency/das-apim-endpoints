using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class IncentiveApplicationDto
    {
        public long LegalEntityId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string SubmittedByEmail { get; set; }
        public string SubmittedByName { get; set; }
        public bool BankDetailsRequired { get; set; }
        public bool NewAgreementRequired { get; set; }
        public IEnumerable<IncentiveApplicationApprenticeshipDto> Apprenticeships { get; set; }

        public static implicit operator IncentiveApplicationDto(IncentiveApplication source)
        {
            return new IncentiveApplicationDto
            {
                LegalEntityId = source.LegalEntityId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                SubmittedByEmail = source.SubmittedByEmail,
                SubmittedByName = source.SubmittedByName,
                BankDetailsRequired = source.BankDetailsRequired,
                NewAgreementRequired = source.NewAgreementRequired,
                Apprenticeships = source.Apprenticeships.Select(x => (IncentiveApplicationApprenticeshipDto)x)
            };
        }
    }
}
