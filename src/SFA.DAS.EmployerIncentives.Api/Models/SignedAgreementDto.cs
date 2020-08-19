using System;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class SignedAgreementDto
    {
        public DateTime SignedDate { get; set; }
        public string SignedByName { get; set; }
        public string SignedByEmail { get; set; }

        public static implicit operator SignedAgreementDto(SignedAgreement from)
        {
            return new SignedAgreementDto
            {
                SignedDate = from.SignedDate,
                SignedByName = from.SignedByName,
                SignedByEmail = from.SignedByEmail
            };
        }
    }
}
