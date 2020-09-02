using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseDetails
{
    public class UpdateVendorRegistrationFormCaseDetailsCommand : IRequest
    {
        private readonly long _legalEntityId;

        public UpdateVendorRegistrationFormCaseDetailsCommand(long legalEntityId)
        {
            _legalEntityId = legalEntityId;
        }
    }
}