using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus
{
    public class UpdateVendorRegistrationFormCaseStatusCommand : IRequest
    {
        public long LegalEntityId { get; }
        public string CaseId { get; }
        public string VendorId { get; }

        public UpdateVendorRegistrationFormCaseStatusCommand(long legalEntityId, string caseId, string vendorId)
        {
            LegalEntityId = legalEntityId;
            CaseId = caseId;
            VendorId = vendorId;
        }
    }
}