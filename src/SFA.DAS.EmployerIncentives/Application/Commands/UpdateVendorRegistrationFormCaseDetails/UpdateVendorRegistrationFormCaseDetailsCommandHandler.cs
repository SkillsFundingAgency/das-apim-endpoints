using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseDetails
{
    public class UpdateVendorRegistrationFormCaseDetailsCommandHandler : IRequestHandler<UpdateVendorRegistrationFormCaseDetailsCommand>
    {
        private readonly ICustomerEngagementFinanceService _financeService;
        private readonly IEmployerIncentivesService _employerIncentivesService;

        private const string CompanyName = "ESFA";
        private const string DefaultCaseStatus = "To Process";

        public UpdateVendorRegistrationFormCaseDetailsCommandHandler(ICustomerEngagementFinanceService financeService, IEmployerIncentivesService employerIncentivesService)
        {
            _financeService = financeService;
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Unit> Handle(UpdateVendorRegistrationFormCaseDetailsCommand command, CancellationToken cancellationToken)
        {
            var vendorDetails = await _financeService.GetVendorByApprenticeshipLegalEntityId(CompanyName, command.HashedLegalEntityId);
            if (vendorDetails != null)
            {
                var updateRequest = new UpdateVendorRegistrationFormRequest
                {
                    VendorId = vendorDetails.VendorIdentifier,
                    CaseId = vendorDetails.RegistrationCaseID,
                    Status = DefaultCaseStatus
                };
                await _employerIncentivesService.UpdateVendorRegistrationFormDetails(command.LegalEntityId, updateRequest);
            }

            return Unit.Value;
        }
    }
}