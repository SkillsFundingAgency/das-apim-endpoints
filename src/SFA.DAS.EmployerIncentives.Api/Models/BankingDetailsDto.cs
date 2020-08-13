using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class BankingDetailsDto
    {
        public long LegalEntityId { get; set; }
        public string VendorCode { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantEmail { get; set; }
        public decimal ApplicationValue { get; set; }

        public static implicit operator BankingDetailsDto(BankingData from)
        {
            return new BankingDetailsDto
            {
                VendorCode = from.VendorCode,
                LegalEntityId = from.LegalEntityId,
                ApplicantEmail = from.ApplicantEmail,
                ApplicationValue = from.ApplicationValue,
                ApplicantName = from.ApplicantName
            };
        }
    }
}