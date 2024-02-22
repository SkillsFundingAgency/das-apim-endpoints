using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus
{
    public class UpdateVendorRegistrationCaseStatusCommand : IRequest
    {
        public long AccountId { get; }
        public long AccountLegalEntityId { get; }
        public string VrfCaseStatus { get; }

        public UpdateVendorRegistrationCaseStatusCommand(long accountId, long accountLegalEntityId, string vrfCaseStatus)
        {
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            VrfCaseStatus = vrfCaseStatus;
        }
    }
}
