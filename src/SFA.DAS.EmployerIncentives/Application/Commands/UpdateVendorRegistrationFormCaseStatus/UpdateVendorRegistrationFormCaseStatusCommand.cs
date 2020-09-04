using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus
{
    public class UpdateVendorRegistrationFormCaseStatusCommand : IRequest
    {
        public long LegalEntityId { get; }
        public string CaseId { get; }

        public UpdateVendorRegistrationFormCaseStatusCommand(long legalEntityId, string caseId)
        {
            LegalEntityId = legalEntityId;
            CaseId = caseId;
        }
    }
}