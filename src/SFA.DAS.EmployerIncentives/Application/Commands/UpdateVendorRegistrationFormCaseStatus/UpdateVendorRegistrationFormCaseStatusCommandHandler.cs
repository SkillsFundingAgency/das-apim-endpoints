using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus
{
    public class UpdateVendorRegistrationFormCaseStatusCommandHandler : IRequestHandler<UpdateVendorRegistrationFormCaseStatusCommand>
    {
        private readonly ICustomerEngagementFinanceService _financeService;
        private readonly IEmployerIncentivesService _employerIncentivesService;
        
        public UpdateVendorRegistrationFormCaseStatusCommandHandler(ICustomerEngagementFinanceService financeService, IEmployerIncentivesService employerIncentivesService)
        {
            _financeService = financeService;
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Unit> Handle(UpdateVendorRegistrationFormCaseStatusCommand command, CancellationToken cancellationToken)
        {
            var statusResponse = await _financeService.GetVendorRegistrationStatusByCaseId(command.CaseId);
            if (statusResponse != null)
            {
                var updateRequest = new UpdateVendorRegistrationFormRequest
                {
                    VendorId = statusResponse.RegistrationCase.SubmittedVendorIdentifier,
                    CaseId = command.CaseId,
                    CaseStatus = statusResponse.RegistrationCase.CaseStatus
                };
                await _employerIncentivesService.UpdateVendorRegistrationFormDetails(command.LegalEntityId, updateRequest);
            }

            return Unit.Value;
        }
    }
}