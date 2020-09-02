using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseDetails
{
    public class UpdateVendorRegistrationFormCaseDetailsCommand : IRequest
    {
        public long LegalEntityId { get; }
        public string HashedLegalEntityId { get; }

        public UpdateVendorRegistrationFormCaseDetailsCommand(long legalEntityId, string hashedLegalEntityId)
        {
            LegalEntityId = legalEntityId;
            HashedLegalEntityId = hashedLegalEntityId;
        }
    }
}