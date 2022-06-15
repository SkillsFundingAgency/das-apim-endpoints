using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class BankingDetailsDto
    {
        public long LegalEntityId { get; set; }
        public string VendorCode { get; set; }
        public string SubmittedByName { get; set; }
        public string SubmittedByEmail { get; set; }
        public decimal ApplicationValue { get; set; }
        public int NumberOfApprenticeships { get; set; }
        public IEnumerable<SignedAgreementDto> SignedAgreements { get; set; }

        public static implicit operator BankingDetailsDto(BankingData from)
        {
            return new BankingDetailsDto
            {
                VendorCode = from.VendorCode,
                LegalEntityId = from.LegalEntityId,
                SubmittedByEmail = from.SubmittedByEmail,
                ApplicationValue = from.ApplicationValue,
                SubmittedByName = from.SubmittedByName,
                NumberOfApprenticeships = from.NumberOfApprenticeships,
                SignedAgreements = from.SignedAgreements.Select(x => (SignedAgreementDto)x)
            };
        }
    }
}